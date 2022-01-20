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

    [Header("ランナーの設定")]
    /// <summary> ランナープレハブ </summary>
    [SerializeField] Runner m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float m_moveDuration = 1f;

    [Space(10)]
    /// <summary>判定処理が終わった時にUIにメッセージを飛ばすイベント</summary>
    [SerializeField] UnityEventWrapperSendText OnDisplayHitZoneMessage = default;

    ///<summary>球の反発係数:プロ野球で使われる公式球を参考にしています</summary> 
    public const float CoefficientOfRestitution = 0.4134f;
    /// <summary>ピッチャーがボールを投げる時に発火されるイベント変数</summary>
    public event Action OnThrowBall = default;
  
    /// <summary>ホームベースに帰ってきたランナーを数える変数</summary>
    int m_countRunnerReturnHomeBase = 0;

    private HitZoneType m_lastHitZoneType;

    /// <summary>現在出塁しているランナーのリスト</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];

    private void Start()
    {
        stageData.Init();
    }

    /// <summary>
    /// ピッチャーが球を投げるのを開始するメンバー関数.
    /// </summary>
    public void PlayBall()
    {
        //判定を初期する.
        m_lastHitZoneType = HitZoneType.None;
      
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
    /// <param name="hitZoneType"></param>
    public void UpdateHitType(HitZoneType hitZoneType)
    {
        m_lastHitZoneType = hitZoneType;
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

        if (m_lastHitZoneType == HitZoneType.None)
        {
            DisplayHitZoneMessage("何処にも当たらなかった.", PlayBall);
        }
        else if(m_lastHitZoneType == HitZoneType.Foul || m_lastHitZoneType == HitZoneType.Out 
            || m_lastHitZoneType == HitZoneType.Catcher)
        {
            //UIにテキストを送って、プレイボール.
            DisplayHitZoneMessage(m_lastHitZoneType.ToString(), PlayBall);
        }
        else
        {
            //UIにテキストを送って、ランナーを走らせる.
            DisplayHitZoneMessage(m_lastHitZoneType.ToString(), () => MoveRunner((int)m_lastHitZoneType));
        }
    }

    /// <summary>
    /// 球のヒットゾーン判定の結果をUIに表示し,その後コールバックを呼び出す関数.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="callBack"></param>
    private void DisplayHitZoneMessage(string text,UnityAction callBack)
    {
        OnDisplayHitZoneMessage?.Invoke(text, callBack);
        Debug.Log(text);
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
    private async UniTask Moving(int hitCount, Runner runner,bool lastRunner = false)
    {
   
        //ヒット数分、ランナーを移動させる.
        for (int i = 0; i < hitCount; i++)
        {
            int nextBaseIndex = runner.currentBaseNumber + 1;
            //ホームベースに着いたら、得点処理をしてランナーを削除
            if (nextBaseIndex == 4)
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
