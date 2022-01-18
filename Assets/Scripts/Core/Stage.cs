using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// ステージクラス.
/// </summary>
[Serializable]
public class Stage 
{
    /// <summary>ステージナンバー</summary>
    public int stageNumber;
    /// <summary>スペシャルステージ（最終ステージ）かどうか判定するフラグ</summary>
    public bool specialStage;
    /// <summary>ピッチャーが投げてくる球種</summary>
    public BallType[] ballTypes;
    /// <summary>ピッチャーが投げてくる球数</summary>
    public int ballNumber;
    /// <summary>ステージクリアに必要な打点数</summary>
    public int clearHitNumer;
}

