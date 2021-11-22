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

    /// <summary>ピッチャーがボールを投げる時に発火される関数をインスペクター上で登録する変数</summary>
    [SerializeField] UnityEventWrapper OnThrowBall = default;


    /// <summary>判定処理が終わった時に飛ばすイベント</summary>
    public event Func<JudgeType, UniTask> OnSendProcessMessage = default;


    private JudgeType m_lastjudgeType;

    bool isFoul = false;//ファール判定が出たら更新しないためのフラグ

    private void Start()
    {
        //game start!!
        PlayBall();
    }


    /// <summary>
    /// ピッチャーが球を投げるのを開始するメンバー関数.
    /// </summary>
    public void PlayBall()
    {
        isFoul = false;
        m_lastjudgeType = JudgeType.None;
        OnThrowBall?.Invoke();
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
    }

    /// <summary>
    /// 球が動かなくなったら呼ばれるメンバー関数.
    /// </summary>
    public void EndMoveBall()
    {
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
        //塁を進む処理
        if (m_lastjudgeType == JudgeType.Hit || m_lastjudgeType == JudgeType.TwoBase || m_lastjudgeType == JudgeType.ThreeBase)
        {
            processtask = HitBall();
        }
        //ホームラン
        else if (m_lastjudgeType == JudgeType.HomeRun)
        {
            processtask = HomeRun();
        }
        //ボールかストライクのカウントする
        else if (m_lastjudgeType == JudgeType.Ball || m_lastjudgeType == JudgeType.Strike || m_lastjudgeType == JudgeType.Foul)
        {
            processtask = CountBallORStrike();
        }
        //アウト
        else if (m_lastjudgeType == JudgeType.Out)
        {
            processtask = Out();
        }

        //uiに判定結果を表示する.
        UniTask uguiTask = OnSendProcessMessage.Invoke(m_lastjudgeType);

        //全ての判定処理が終了するのを待つ.
        await UniTask.WhenAll(processtask, uguiTask);
        Debug.Log("end process..");

        //次の球を投げる.
        OnThrowBall?.Invoke();
    }

    public async UniTask HitBall()
    {
        await UniTask.WaitForEndOfFrame();
    }

    public async UniTask HomeRun()
    {
        await UniTask.WaitForEndOfFrame();
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
    None, Strike, Ball, Hit, TwoBase, ThreeBase, HomeRun, Foul, Out
}