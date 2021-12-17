using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

/// <summary>
/// �v���C���[�f�[�^�N���X
/// </summary>
[Serializable]
public class PlayerRecord
{
    public int score;
    public string name;

    /// <summary>
    /// �R���X�g���N�^
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
/// �����L���O�{�[�h�ɍڂ���e�L�X�g���b�V���N���X���܂Ƃ߂��N���X.
/// </summary>
[Serializable]
public class PlayerRecordText
{
    [SerializeField] TextMeshProUGUI m_posText;
    [SerializeField] TextMeshProUGUI m_scoreText;
    [SerializeField] TextMeshProUGUI m_nameText;

    /// <summary>
    ///�@�X�R�A�ƃv���C���[���������L���O�{�[�h�ɕ\������֌W.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="name"></param>
    public void SetPlayerData(PlayerRecord playerRecord)
    {
        m_scoreText.text = playerRecord.score.ToString();
        m_nameText.text = playerRecord.name.ToString();
    }

    /// <summary>
    /// �����L���O�̏����\������֐�.
    /// </summary>
    /// <param name="rankPos"></param>
    public void SetPosition(int rankPos)
    {
        m_posText.text = rankPos.ToString();
    }
}

/// <summary>
/// �����L���O�f�[�^�N���X.
/// Json�`���ŕۑ������N���X�ł�.
/// </summary>
[Serializable]
public class RankingData : JsonCutomUtility.ISerializeToJson
{
   public List<PlayerRecord> playerRecordList = new List<PlayerRecord>();
}

/// <summary>
/// �����L���O���Ǘ�����N���X.
/// </summary>
public class RankingSystem : MonoBehaviour
{
    [SerializeField] PlayerRecordText[] m_playerRecordTexts;

    private RankingData m_rankingData;

    /// <summary>�����L���O�ɕ\������v���C���[�f�[�^�̍ő吔</summary>
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
    /// �����L���O�f�[�^�̏���������֐�.
    /// </summary>
    private void Init()
    {
        //�����L���O�̃Z�[�u�f�[�^������Ȃ烍�[�h����B�Ȃ��Ȃ珉��������.
        if (!JsonCutomUtility.LoadDataFromJson(path, ref m_rankingData))
        {
            for (int i = 0; i < maxDataNumber; i++)
            {
                m_rankingData.playerRecordList.Add(new PlayerRecord());
            }
        }
    }

    /// <summary>
    /// �v���C���[�f�[�^��ǉ�����֐�.
    /// </summary>
    /// <param name="score"></param>
    /// <param name="playerName"></param>
    public void AddNewRecord(int score, string playerName)
    {
        //�ŉ��ʂ̃f�[�^�����������Ă���
        var minimumScoreData = m_rankingData.playerRecordList.LastOrDefault();

        //�X�R�A����ԒႢ�f�[�^�Ɣ�r���āA�X�R�A������ȉ��Ȃ烊�X�g�ɉ����Ȃ�.
        if (score <= minimumScoreData.score) return;

        //�X�R�A�ŉ��ʂ̃f�[�^���폜����.
        m_rankingData.playerRecordList.Remove(minimumScoreData);

        //�V�����f�[�^�����X�g�ɉ�����.
        m_rankingData.playerRecordList.Add(new PlayerRecord(score, playerName));

        //�\�[�g����
        SortData();
    }


    /// <summary>
    /// �v���C���[�f�[�^�̊l���N�����Ƀ\�[�g����֐�.
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
    /// m_playerRecordList����ɁA�����L���O�{�[�h�ɋL�^��\�����A���ʂ�t����֐�.
    /// </summary>
    private void DisplayPlayerDatas()
    {
        //�\�[�g����
        SortData();

        int lastRankPos = 1;
        //display each data.
        for (int i = 0; i < maxDataNumber; i++)
        {
            m_playerRecordTexts[i].SetPlayerData(m_rankingData.playerRecordList[i]);

            //�X�R�A���O�̃v���C���[�Ɠ����Ȃ�A�O�̃v���C���[�Ɠ������ʂɂ���.�����łȂ���΁A�����L���O�ʒu�Ɠ������ʂ�t����.
            if (i > 0 && m_rankingData.playerRecordList[i].score != m_rankingData.playerRecordList[i - 1].score)
            {
                lastRankPos = i + 1;
            }
            m_playerRecordTexts[i].SetPosition(lastRankPos);
        }

        //�Z�[�u����
        JsonCutomUtility.SaveDataToJson(path, m_rankingData);
    }
}
