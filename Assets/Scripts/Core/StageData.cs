using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StageData",menuName ="CreateStageDataBase")]
public class StageData : ScriptableObject 
{
    [SerializeField] Stage[] stages;
    public Stage[] GetStageArray => stages;
}

/// <summary>
/// ステージクラス.
/// </summary>
[Serializable]
public class Stage
{
    /// <summary>ステージナンバー</summary>
    public int stageNumber;
    /// <summary>ピッチャーが投げてくる球種</summary>
    public BallType[] ballTypes;
    /// <summary>ピッチャーが投げてくる球数</summary>
    public int capacityOfBall;
    /// <summary>ステージクリアに必要な打点数</summary>
    public int clearScore;
}

