using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 野球のルールに従って,ゲームを進行を管理するクラス
/// </summary>
public class BaseBallLogic : SingletonMonoBehaviour<BaseBallLogic>
{

    /// <summary>ピッチャーがボールを投げる時に発火される関数をインスペクター上で登録する変数</summary>
    [SerializeField] UnityEventWrapper OnThrowBall = default;

    public event Action<JudgeType> OnReceiveMessage = default;
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
        OnReceiveMessage?.Invoke(m_lastjudgeType);
        Process(m_lastjudgeType);
        //次の球を投げるまでの待ち時間を実装すること
        OnThrowBall?.Invoke();
    }

    /// <summary>
    ///　ピッチャーが投げた後の球の行方に応じて、処理するメンバー関数.
    /// </summary>
    /// <param name="judgeType"></param>
    private void Process(JudgeType judgeType)
    {
        switch (judgeType)
        {
            case JudgeType.None:
                break;
            case JudgeType.Strike:
                break;
            case JudgeType.Ball:
                break;
            case JudgeType.Hit:
                break;
            case JudgeType.TwoBase:
                break;
            case JudgeType.ThreeBase:
                break;
            case JudgeType.HomeRun:
                break;
            case JudgeType.Foul:
                break;
            case JudgeType.Out:
                break;
            default:
                break;
        }
    }
}


/// <summary>
/// ピッチャーが投げた後の判定
/// </summary>
public enum JudgeType
{
    None,Strike, Ball, Hit, TwoBase, ThreeBase, HomeRun, Foul, Out
}