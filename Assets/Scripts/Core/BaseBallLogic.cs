using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Cysharp.Threading.Tasks;


/// <summary>
/// 野球のルールに従って,ゲームを進行を管理するクラス
/// </summary>
public class BaseBallLogic : SingletonMonoBehaviour<BaseBallLogic>
{
    ///<summary>球の反発係数:プロ野球で使われる公式球を参考にしています</summary> 
    public const float CoefficientOfRestitution = 0.4134f;

    /// <summary>ピッチャーがボールを投げる時に発火されるイベント変数</summary>
    public event Action OnThrowBall = default;
    /// <summary>判定処理が終わった時にUIにメッセージを飛ばすイベント</summary>
    public event Func<JudgeType, UniTask> OnSendProcessMessage = default;
    /// <summary>ボールの打球判定が終わったらランナーを走らせるイベント</summary>
    public event Func<JudgeType, UniTask> OnProcessRunner = default;

    private JudgeType m_lastjudgeType;

    bool isFoul = false;//ファール判定が出たら更新しないためのフラグ

    [Header("実行後にチェックを入れるとPlayBall。一度チェック入れたらいじらないこと")]
    public bool isDebug = false;
    private bool init = true;

    float timer = 0f;
    private void Update()
    {
        if (isDebug && init)
        {
            PlayBall();
            init = false;
        }

        if(s)
        {
            timer += Time.deltaTime;
            Debug.Log((int)timer);
        }
        else
        {
            timer = 0f;
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
        //Funcの初期化処理.
        OnProcessRunner += InitFuncMethod;
        OnSendProcessMessage += InitFuncMethod;
        //ボールを投げる.
        OnThrowBall?.Invoke();
    }

    bool s = false;
    /// <summary>
    /// FuckとUniTaskを使ってイベント処理を行う際のデフォルトメソッド.
    /// </summary>
    /// <param name="arg"></param>
    /// <returns></returns>
    private async UniTask InitFuncMethod(JudgeType arg)
    {
        //await UniTask.WaitForEndOfFrame();
        s = true;
        await UniTask.Delay(System.TimeSpan.FromSeconds(5f));
        Debug.Log("Initialize func methods...");
        s = false;
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
        Debug.Log("end ball :" + m_lastjudgeType.ToString());
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
    private async void Process()
    {
        UniTask processtask = default;
       
        //ボールかストライクのカウントする
        if (m_lastjudgeType == JudgeType.Ball || m_lastjudgeType == JudgeType.Strike || m_lastjudgeType == JudgeType.Foul)
        {
            processtask = CountBallORStrike();
        }
        //アウト
        else if (m_lastjudgeType == JudgeType.Out)
        {
            processtask = Out();
        }
        //ヒット　or ホームラン時
        else
        {
           processtask = OnProcessRunner.Invoke(m_lastjudgeType);
        }

        //uiに判定結果を表示する.//ランナーを走らせる
        await UniTask.WhenAll(OnSendProcessMessage.Invoke(m_lastjudgeType),processtask);
      
        //次の球を投げる.
        PlayBall();
    }

  
    public async UniTask CountBallORStrike()
    {
        await UniTask.WaitForEndOfFrame();
    }

    public async UniTask Out()
    {
        await UniTask.WaitForEndOfFrame();
    }
}


/// <summary>
/// ピッチャーが投げた後の判定
/// </summary>
public enum JudgeType
{
    None, Strike, Ball, Hit, TwoBase, ThreeBase, HomeRun, Foul, Out, Catcher, Pitcher,OffThePremises
}