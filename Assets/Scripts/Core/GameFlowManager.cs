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
    [SerializeField] StageData stageData = default;

    ///<summary>球の反発係数:プロ野球で使われる公式球を参考にしています</summary> 
    public const float CoefficientOfRestitution = 0.4134f;
    /// <summary>ピッチャーがボールを投げる時に発火されるイベント変数</summary>
    public event Action OnThrowBall = default;
    /// <summary>判定処理が終わった時にUIにメッセージを飛ばすイベント</summary>
    public event Action<JudgeType> OnSendProcessMessage = default;

    private JudgeType m_lastjudgeType;

    bool isFoul = false;//ファール判定が出たら更新しないためのフラグ

    [Header("実行後にチェックを入れるとPlayBall。一度チェック入れたらいじらないこと")]
    public bool isDebug = false;
    private bool init = true;

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
        //判定周りのフラグを初期化.
        isFoul = false;
        m_lastjudgeType = JudgeType.None;


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
    /// 打球の判定を更新するメンバー関数.
    /// </summary>
    /// <param name="judgeType"></param>
    public void UpdateJudgeType(JudgeType judgeType)
    {
        if (isFoul) return;

        if (judgeType == JudgeType.Foul)
        {
            m_lastjudgeType = judgeType;
            isFoul = true;
        }
        else
        {
            m_lastjudgeType = judgeType;
        }
        Debug.Log(m_lastjudgeType.ToString());
    }

    /// <summary>
    /// 球が動かなくなったら呼ばれるメンバー関数.
    /// </summary>
    public async void EndMoveBall()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1f), ignoreTimeScale: false);
        stageData.currentBallLeftNumer--;
        if (m_lastjudgeType == JudgeType.None)
        {
            Debug.LogWarning("判定結果なし");
            PlayBall();
        }
        else
        {
            Process();
        }
    }

    /// <summary>
    ///　ピッチャーが投げた後の球の行方に応じて、処理するメンバー関数.
    /// </summary>
    /// <param name="judgeType"></param>
    private void Process()
    {
        //uiに判定結果を表示する.
        OnSendProcessMessage?.Invoke(m_lastjudgeType);

        //ヒットした
        if (m_lastjudgeType == JudgeType.Hit || m_lastjudgeType == JudgeType.TwoBase
            || m_lastjudgeType == JudgeType.ThreeBase || m_lastjudgeType == JudgeType.HomeRun)
        {
            //ランナーを走らせる
            ProcessRunner(m_lastjudgeType);
        }
        else
        {
            //次の球を投げる.
            PlayBall();
        }
    }

    /// <summary> ランナープレハブ </summary>
    [SerializeField] GameObject m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float m_moveDuration = 1f;

    /// <summary>現在出塁しているランナーのリスト</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];

    /// <summary>ホームベースに帰ってきたランナーを数える変数</summary>
    int m_countRunnerReturnHomeBase = 0;


    private void ProcessRunner(JudgeType type)
    {
        switch (type)
        {
            case JudgeType.Hit:
                MoveRunner(1);
                break;
            case JudgeType.TwoBase:
                MoveRunner(2);
                break;
            case JudgeType.ThreeBase:
                MoveRunner(3);
                break;
            case JudgeType.HomeRun:
                MoveRunner(4);
                break;
        }
    }

    /// <summary>
    /// ランナーを動かす命令を発行する関数.
    /// </summary>
    /// <param name="hitNumber">ヒット=1、ツーベースヒット=2、スリーベースヒット=3、ホームラン=4</param>
    public void MoveRunner(int hitNumber)
    {
        //現在出塁しているランナーを先ずは走らせる.
        if (m_currentRunner.Count > 0)
        {
            foreach (Runner runner in m_currentRunner)
            {
                Moving(hitNumber, runner).Forget();
            }
        }

        //新しく出塁するランナーをインスタンス化させて、ヒット数だけ走らせる.
        Runner newRunner = Instantiate(m_runnerSourcePrefab, m_basePostions[0].position, Quaternion.identity).AddComponent<Runner>();
        Moving(hitNumber, newRunner, true).Forget();

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
/// <summary>
/// ピッチャーが投げた後の判定
/// </summary>
public enum JudgeType
{
    None, Strike, Ball, Hit, TwoBase, ThreeBase, HomeRun, Foul, Out, Catcher, Pitcher, OffThePremises
}