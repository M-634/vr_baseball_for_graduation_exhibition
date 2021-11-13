using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

/// <summary>
/// 野球のルールに従って、ピッチャーが投げた後の球の判定に応じてゲームを進行するクラス
/// </summary>
public class BaseBallLogicEventManager : SingletonMonoBehaviour<BaseBallLogicEventManager>
{
    public event Action<JudgeType> OnReceiveMessage = default;

    public void SendMessage(JudgeType judgeType)
    {
        Process(judgeType);
        OnReceiveMessage?.Invoke(judgeType);
    }

    private void Process(JudgeType judgeType)
    {
        switch (judgeType)
        {
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
    Strike, Ball, Hit, TwoBase, ThreeBase, HomeRun, Foul, Out
}