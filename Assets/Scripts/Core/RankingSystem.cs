using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

/// <summary>
/// プレイヤーデータクラス
/// </summary>
[Serializable]
public class PlayerRecord
{
    public int score;
    public string name;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="score"></param>
    /// <param name="name"></param>
    public PlayerRecord(int score = 0, string name = "")
    {
        this.score = score;
        this.name = name;
    }
}

/// <summary>
/// ランキングボードに載せるテキストメッシュクラスをまとめたクラス.
/// </summary>
[Serializable]
public class PlayerRecordText
{
    [SerializeField] TextMeshProUGUI m_posText;
    [SerializeField] TextMeshProUGUI m_scoreText;
    [SerializeField] TextMeshProUGUI m_nameText;

    /// <summary>
    ///　スコアとプレイヤー名をランキングボードに表示する関係.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="name"></param>
    public void SetPlayerData(PlayerRecord playerRecord)
    {
        m_scoreText.text = playerRecord.score.ToString();
        m_nameText.text = playerRecord.name.ToString();
    }

    /// <summary>
    /// ランキングの序列を表示する関数.
    /// </summary>
    /// <param name="rankPos"></param>
    public void SetPosition(int rankPos)
    {
        m_posText.text = rankPos.ToString();
    }
}

/// <summary>
/// ランキングデータクラス.
/// Json形式で保存されるクラスです.
/// </summary>
[Serializable]
public class RankingData : JsonCutomUtility.ISerializeToJson
{
   public List<PlayerRecord> playerRecordList = new List<PlayerRecord>();
}

/// <summary>
/// ランキングを管理するクラス.
/// </summary>
public class RankingSystem : MonoBehaviour
{
    [SerializeField] PlayerRecordText[] m_playerRecordTexts;

    private RankingData m_rankingData;

    /// <summary>ランキングに表示するプレイヤーデータの最大数</summary>
    const int maxDataNumber = 5;
    string path;

    private void Start()
    {
#if UNITY_EDITOR
        path = Application.dataPath + "/RankingData.json";
#else
        path = Application.persistentDataPath + "/RankingData.json"; 
#endif

        Init();

        DisplayPlayerDatas();
    }

    /// <summary>
    /// ランキングデータの初期化する関数.
    /// </summary>
    private void Init()
    {
        //ランキングのセーブデータがあるならロードする。ないなら初期化する.
        if (!JsonCutomUtility.LoadDataFromJson(path, ref m_rankingData))
        {
            for (int i = 0; i < maxDataNumber; i++)
            {
                m_rankingData.playerRecordList.Add(new PlayerRecord());
            }
        }
    }

    /// <summary>
    /// プレイヤーデータを追加する関数.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="playerName"></param>
    public void AddNewRecord(int score, string playerName)
    {
        //最下位のデータを引っ張ってくる
        var minimumScoreData = m_rankingData.playerRecordList.LastOrDefault();

        //スコアが一番低いデータと比較して、スコアがそれ以下ならリストに加えない.
        if (score <= minimumScoreData.score) return;

        //スコア最下位のデータを削除する.
        m_rankingData.playerRecordList.Remove(minimumScoreData);

        //新しいデータをリストに加える.
        m_rankingData.playerRecordList.Add(new PlayerRecord(score, playerName));

        //ソートする
        SortData();
    }


    /// <summary>
    /// プレイヤーデータの獲得年俸を基準にソートする関数.
    /// </summary>
    private void SortData()
    {
        //sort by getScore;
        m_rankingData.playerRecordList.Sort((e1, e2) =>
        {
            return e2.score.CompareTo(e1.score);
        });
    }

    /// <summary>
    /// m_playerRecordListを基に、ランキングボードに記録を表示し、順位を付ける関数.
    /// </summary>
    private void DisplayPlayerDatas()
    {
        //ソートする
        SortData();

        int lastRankPos = 1;
        //display each data.
        for (int i = 0; i < maxDataNumber; i++)
        {
            m_playerRecordTexts[i].SetPlayerData(m_rankingData.playerRecordList[i]);

            //スコアが前のプレイヤーと同じなら、前のプレイヤーと同じ順位にする.そうでなければ、ランキング位置と同じ順位を付ける.
            if (i > 0 && m_rankingData.playerRecordList[i].score != m_rankingData.playerRecordList[i - 1].score)
            {
                lastRankPos = i + 1;
            }
            m_playerRecordTexts[i].SetPosition(lastRankPos);
        }

        //セーブする
        JsonCutomUtility.SaveDataToJson(path, m_rankingData);
    }
}
