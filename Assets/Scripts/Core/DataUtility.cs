using System;

//データクラスをまとめたファイル.

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


/// <summary>
/// リザルト.
/// </summary>
[Serializable]
public class Result
{
    /// <summary>ヒット</summary>
    public int hitCount;
    /// <summary>ツーラン</summary>
    public int twoBaseCount;
    /// <summary>スリーラン</summary>
    public int threeBaseCount;
    /// <summary>ホームラン</summary>
    public int homeRunCount;
    /// <summary>ストライク</summary>
    public int strikeCount;

    /// <summary>最大距離</summary>
    public float maxDistance;
    /// <summary>合計距離</summary>
    public float sumDistance;

    /// <summary>報酬金額</summary>
    public int amountOfRemuneration;
    /// <summary>累計報酬金額</summary>
    public int accumulatedRemuneration;


    /// <summary>
    /// 初期化:ステージが変化する時に初期化すること.
    /// <para>累計報酬金額(= accumulatedRemuneration)は、初期ステージ以外は初期されない.</para> 
    /// </summary>
    public void Init(bool isFirstStage = false)
    {
        hitCount = 0;
        twoBaseCount = 0;
        threeBaseCount = 0;
        homeRunCount = 0;
        strikeCount = 0;

        maxDistance = 0f;
        sumDistance = 0f;

        amountOfRemuneration = 0;

        if (isFirstStage)
        {
            accumulatedRemuneration = 0;
        }
    }
}


