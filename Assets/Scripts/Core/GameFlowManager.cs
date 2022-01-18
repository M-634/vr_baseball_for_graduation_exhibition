using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;


/// <summary>
/// 野球のルールに従って,ゲームを進行を管理するクラス
/// </summary>
public class GameFlowManager : SingletonMonoBehaviour<GameFlowManager>
{
    [Header("ステージデータ")]
    [SerializeField] StageData stageData = default;

    [Header("実行後にチェックを入れるとPlayBall。一度チェック入れたらいじらないこと")]
    public bool isDebug = false;


    private bool init = true;


    [Header("ランナーの設定")]
    /// <summary> ランナープレハブ </summary>
    [SerializeField] Runner m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float m_moveDuration = 1f;

    /// <summary>現在出塁しているランナーのリスト</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];

    /// <summary>ホームベースに帰ってきたランナーを数える変数</summary>
    int m_countRunnerReturnHomeBase = 0;



    ///<summary>球の反発係数:プロ野球で使われる公式球を参考にしています</summary> 
    public const float CoefficientOfRestitution = 0.4134f;
    /// <summary>ピッチャーがボールを投げる時に発火されるイベント変数</summary>
    public event Action OnThrowBall = default;
    /// <summary>判定処理が終わった時にUIにメッセージを飛ばすイベント</summary>
    public event Action<string, UnityAction> OnSendProcessMessage = default;

    private HitType m_lastHitType;

    public bool Out { get; set; }
    public bool Strike { get; set; }
    public bool Foul { get; set; }

    private void Start()
    {
        stageData.Init();
    }

    private void Update()
    {
        if (isDebug && init)
        {
            PlayBall();
            init = false;
        }
    }

    /// <summary>
    /// ピッチャーが球を投げるのを開始するメンバー関数.
    /// </summary>
    public void PlayBall()
    {
        //判定を初期する.
        m_lastHitType = HitType.None;
        Out = false;
        Strike = false;
        Foul = false;

        if (stageData.currentBallLeftNumer > 0)
        {
            //ボールを投げる.
            OnThrowBall?.Invoke();
        }
        else
        {
            Debug.Log("GAME OVER...");
            stageData.Init();
        }

    }

    /// <summary>
    /// ヒット判定を更新する関数.
    /// </summary>
    /// <param name="hitType"></param>
    public void UpdateHitType(HitType hitType)
    {
        m_lastHitType = hitType;
    }

    /// <summary>
    /// 球が動かなくなったら呼ばれるメンバー関数.
    /// </summary>
    public async void EndMoveBall()
    {
        //判定結果を出すまで、遅延させる.
        await UniTask.Delay(TimeSpan.FromSeconds(1f), ignoreTimeScale: false);

        //各ステージの残りの球数を更新する.
        stageData.currentBallLeftNumer--;

        if (m_lastHitType == HitType.None)
        {
            //ストライク
            if (Strike)
            {
                OnSendProcessMessage.Invoke("Strike", PlayBall);
            }
            //アウト
            else if (Out)
            {
                OnSendProcessMessage.Invoke("Out", PlayBall);
            }
            //ファール
            else if (Foul)
            {
                OnSendProcessMessage.Invoke("Foul", PlayBall);
            }
        }
        //ホームランなら特別なイベントを発生させて、ランナーを走らせる
        else if (m_lastHitType == HitType.HomeRun)
        {
            OnSendProcessMessage.Invoke("HomeRun", () => MoveRunner(4));
        }
        //ヒットしたら、ランナーを走らせる
        else
        {
            OnSendProcessMessage.Invoke(m_lastHitType.ToString(), () => MoveRunner((int)m_lastHitType));
        }
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
    /// <param name="hitNumber"></param>
    /// <param name="runner"></param>
    private async UniTask Moving(int hitNumber, Runner runner, bool lastRunner = false)
    {
        //ヒット数分、ランナーを移動させる.
        for (int i = 0; i < hitNumber; i++)
        {
            int nextBaseIndex = runner.currentBaseNumber + 1;
            //ホームベースに着いたら、得点処理をしてランナーを削除
            if (nextBaseIndex > 3)
            {
                m_countRunnerReturnHomeBase++;
                runner.Move(m_basePostions[0], m_moveDuration, () =>
                 {
                     DeleteRunner(runner);
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
            EndMove();
        }
    }

    /// <summary>
    /// ランナー処理が終わったら呼ばれる関数.
    /// </summary>
    private void EndMove()
    {
        Debug.Log("runner　処理が終わった");
        if (m_countRunnerReturnHomeBase >= stageData.CurrentStageData.clearHitNumer)
        {
            Debug.Log("stage clear");
            ResetRunner();

            if (stageData.CurrentStageData.specialStage)
            {
                Debug.Log("Game Clear");
            }
            else
            {
                //次のステージへ
                stageData.currentStageNumber++;
                stageData.currentBallLeftNumer = stageData.CurrentStageData.ballNumber;
                m_countRunnerReturnHomeBase = 0;
                PlayBall();
            }
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

}
