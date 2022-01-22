using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;

//このクラスの責務が多すぎる.暇がある時にリファクタリングしようと思います(優先度は低め)

/// <summary>
/// 野球のルールに従って,ゲームを進行を管理するクラス
/// </summary>
public class GameFlowManager : SingletonMonoBehaviour<GameFlowManager>
{
    #region field
    #region stage
    [Header("ステージデータ")]
    [SerializeField] StageData m_stageData = default;

    private int m_leftBallCount;
    private Stage m_currentStage = null;

    private Subject<int> m_leftBallCountSubject = new Subject<int>();
    private Subject<Stage> m_currentStageSubject = new Subject<Stage>();

    public bool IsLastStage => GetCurrentStage.stageNumber == m_stageData.GetStageArray.Length - 1;
    public IObservable<Stage> OnCurrentStageChanged => m_currentStageSubject;
    public IObservable<int> OnLeftBallCountChanged => m_leftBallCountSubject;

    public int LeftBallCount
    {
        get => m_leftBallCount;
        private set
        {
            m_leftBallCount = value;
            if (m_leftBallCount > -1)
            {
                m_leftBallCountSubject.OnNext(m_leftBallCount);
            }
        }
    }

    public Stage GetCurrentStage
    {
        get => m_currentStage;
        private set
        {
            //ステージ変更時,ステージ情報を表示するUIの値を初期化する
            m_currentStage = value;
            GetScore = 0;
            m_result.Init();
            LeftBallCount = m_currentStage.capacityOfBall;
            m_currentStageSubject.OnNext(m_currentStage);
        }
    }
    #endregion

    #region runner
    [Header("ランナーの設定")]
    /// <summary> ランナープレハブ </summary>
    [SerializeField] Runner m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float m_moveDuration = 1f;

    /// <summary>ホームベースに帰ってきたランナーを数える変数</summary>
    int m_getScore = 0;

    Subject<int> m_getScoreSubject = new Subject<int>();

    /// <summary>現在出塁しているランナーのリスト</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];
    public IObservable<int> OnGetScoreChanged => m_getScoreSubject;

    public int GetScore
    {
        get => m_getScore;
        set
        {
            m_getScore = value;
            m_getScoreSubject.OnNext(m_getScore);
        }
    }
    #endregion

    [Space(10)]
    /// <summary>判定処理が終わった時にWorldSpace上のUIにメッセージを飛ばすイベント</summary>
    [SerializeField] UnityEventWrapperSendText OnDisplayMessage = default;
    /// <summary>ステージクリア後にWorldSpace上のUIにリザルトを飛ばすイベント</summary>
    [SerializeField] UnityEventWrapperDisplayResult OnDisplayResult = default;
    ///<summary>球の反発係数:プロ野球で使われる公式球を参考にしています</summary> 
    public const float CoefficientOfRestitution = 0.4134f;
    /// <summary>ピッチャーがボールを投げる時に発火されるイベント変数</summary>
    public event Action<BallType[]> OnThrowBall = default;

    private HitZoneType m_lastHitZoneType;

    private readonly Result m_result = new Result();
    #endregion

    #region method
    /// <summary>
    /// 初期化する関数
    /// </summary>
    private void Initialize()
    {
        GetCurrentStage = m_stageData.GetStageArray[0];
        m_result.Init(true);
        ResetRunner();
    }

    /// <summary>
    /// ステージクリア時に呼ばれる関数.
    /// </summary>
    private void ClearStage()
    {
        //最終ステージクリア
        if (IsLastStage)
        {
            EndStage("GameClear", () =>
            {
                //ランキング登録

                //リスタート
            });
        }
        //ステージクリア
        else
        {
            EndStage("ClearStage", () =>
            {
                //次のステージへ
                GetCurrentStage = m_stageData.GetStageArray[GetCurrentStage.stageNumber + 1];
                PlayBall(false, true);
            });
        }
    }

    /// <summary>
    /// ステージ終了後,UIへメッセージを飛ばし、リザルトを表示する。
    /// その後、登録したコールバックを呼び出す関数.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callBack"></param>
    private void EndStage(string message, UnityAction callBack)
    {
        //メッセージを飛ばす.
        OnDisplayMessage?.Invoke(message, () =>
        {
            //ランナーを削除
            ResetRunner();
            //リザルト表示.
            OnDisplayResult?.Invoke(m_result, () =>
            {
                //コールバック
                callBack.Invoke();
            });
        });
        Debug.Log(message);
    }

    /// <summary>
    /// ピッチャーが球を投げる関数.
    /// </summary>
    /// <param name="isFirstStage">初めのステージかどうか判定するフラグ</param>
    /// <param name="isFirstBall">各ステージごとの初球かどうか判定するフラグ</param>
    public void PlayBall(bool isFirstStage = false, bool isFirstBall = false)
    {
        //判定を初期する.
        m_lastHitZoneType = HitZoneType.None;

        //初めのステージなら初期化する
        if (isFirstStage) Initialize();

        //初級以外は、残りの球数を減らす
        if (!isFirstBall) LeftBallCount--;

        //残りの球数がなくなったらゲームオーバー
        if (LeftBallCount < 0)
        {
            EndStage("GameOver", () =>
             {
                 //リスタート
             });
            return;
        }
        //ピッチャーが球を投げる
        OnThrowBall?.Invoke(GetCurrentStage.ballTypes);
    }

    /// <summary>
    /// ヒット判定を更新する関数.
    /// </summary>
    /// <param name="hitZoneType"></param>
    public void UpdateHitType(HitZoneType hitZoneType)
    {
        m_lastHitZoneType = hitZoneType;
    }

    /// <summary>
    /// 球が動かなくなったら呼ばれるメンバー関数.
    /// </summary>
    /// <param name="endBallPos"></param>
    public async void EndMoveBall(Vector3 endBallPos)
    {
        //判定結果を出すまで、遅延させる.
        await UniTask.Delay(TimeSpan.FromSeconds(1f), ignoreTimeScale: false);

        if (m_lastHitZoneType == HitZoneType.None)
        {
            DisplayMessage("何処にも当たらなかった.", () => PlayBall());
        }
        else if (m_lastHitZoneType == HitZoneType.Foul || m_lastHitZoneType == HitZoneType.Out
            || m_lastHitZoneType == HitZoneType.Catcher)
        {
            //UIにテキストを送って、プレイボール.
            DisplayMessage(m_lastHitZoneType.ToString(), () => PlayBall());
        }
        else
        {
            //UIにテキストを送って、ランナーを走らせる.
            DisplayMessage(m_lastHitZoneType.ToString(), () => MoveRunner((int)m_lastHitZoneType));
        }
        //リザルトを更新する
        UpdateResult(m_lastHitZoneType, endBallPos);
    }

    /// <summary>
    /// 球のヒットゾーン判定の結果をUIに表示し,その後コールバックを呼び出す関数.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="callBack"></param>
    private void DisplayMessage(string text, UnityAction callBack)
    {
        OnDisplayMessage?.Invoke(text, callBack);
        Debug.Log(text);
    }

    /// <summary>
    /// リザルトを更新する関数
    /// </summary>
    private void UpdateResult(HitZoneType hitZoneType, Vector3 endBallPos)
    {
        //各種カウンターを更新する.
        if (hitZoneType == HitZoneType.Hit)
        {
            m_result.hitCount++;
        }
        else if (hitZoneType == HitZoneType.TwoBaseHit)
        {
            m_result.twoBaseCount++;
        }
        else if (hitZoneType == HitZoneType.ThreeBaseHit)
        {
            m_result.threeBaseCount++;
        }
        else if (hitZoneType == HitZoneType.HomeRun)
        {
            m_result.homeRunCount++;
        }
        else if (hitZoneType == HitZoneType.Catcher)
        {
            m_result.strikeCount++;
        }
        //距離を更新する
        CalcDistance(endBallPos);
    }

    /// <summary>
    /// 最大飛距離と合計飛距離を計算する関数
    /// </summary>
    /// <param name="endBallPos"></param>
    /// <returns></returns>
    private void CalcDistance(Vector3 endBallPos)
    {
        float distance = Vector3.Distance(GetHomeBase.position, endBallPos);

        if (distance > m_result.maxDistance)
        {
            m_result.maxDistance = distance;
        }
        m_result.sumDistance += distance;
    }



    /// <summary>
    /// ランナーを動かす命令を発行する関数.
    /// </summary>
    /// <param name="hitCount">ヒット=1、ツーベースヒット=2、スリーベースヒット=3、ホームラン=4</param>
    public void MoveRunner(int hitCount)
    {
        //現在出塁しているランナーを先ずは走らせる.
        if (m_currentRunner.Count > 0)
        {
            foreach (Runner runner in m_currentRunner)
            {
                Moving(hitCount, runner).Forget();
            }
        }

        //新しく出塁するランナーをインスタンスする.
        Runner newRunner = Instantiate(m_runnerSourcePrefab, transform);
        newRunner.transform.SetPositionAndRotation(m_basePostions[0].position, Quaternion.identity);

        //ランナーを走らせる
        Moving(hitCount, newRunner, true).Forget();

        //出塁ランナーリストへ追加する.
        m_currentRunner.Add(newRunner);
    }

    /// <summary>
    /// 実際にランナーをヒット数だけ走らせる関数.
    /// </summary>
    /// <param name="hitCount"></param>
    /// <param name="runner"></param>
    private async UniTask Moving(int hitCount, Runner runner, bool lastRunner = false)
    {

        //ヒット数分、ランナーを移動させる.
        for (int i = 0; i < hitCount; i++)
        {
            int nextBaseIndex = runner.currentBaseNumber + 1;
            //ホームベースに着いたら、得点処理をしてランナーを削除
            if (nextBaseIndex == 4)
            {
                runner.Move(m_basePostions[0], m_moveDuration, () =>
                 {
                     DeleteRunner(runner);
                     GetScore++;
                 });
                break;
            }
            else
            {
                runner.Move(m_basePostions[nextBaseIndex], m_moveDuration);
                runner.currentBaseNumber++;
            }
            await UniTask.Delay(TimeSpan.FromSeconds(m_moveDuration));
        }

        if (lastRunner)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(m_moveDuration));
            EndMove();
        }

    }

    /// <summary>
    /// ランナー処理が終わったら呼ばれる関数.
    /// </summary>
    private void EndMove()
    {
        Debug.Log("runner　処理が終わった");
        if (GetScore >= GetCurrentStage.clearScore)
        {
            ClearStage();
        }
        else
        {
            PlayBall();
        }
    }

    /// <summary>
    /// 引数に指定したランナーを削除する関数.
    /// </summary>
    /// <param name="runner"></param>
    private void DeleteRunner(Runner runner)
    {
        m_currentRunner.Remove(runner);
        Destroy(runner.gameObject);
    }

    /// <summary>
    /// 出塁している全てのランナーを削除する関数.
    /// </summary>
    public void ResetRunner()
    {
        if (m_currentRunner.Count == 0) return;

        foreach (var runner in FindObjectsOfType<Runner>())
        {
            DeleteRunner(runner);
        }

    }
    #endregion
}
