using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

/// <summary>
/// WorldSpace���UI���䂷��N���X
/// </summary>
public class UGUIControl : MonoBehaviour
{
    /// <summary>���̔��茋�ʂ�\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI m_judgmentResultOfBall = default;

    /// <summary>�X�^�[�g�{�^��</summary>
    [SerializeField] Button m_startButton = default;

    [Header("StageSatus")]
    /// <summary>�Q�[�����̊e�X�e�[�W�̃X�e�[�^�X��UI�ɕ\������I�u�W�F�b�g</summary>
    [SerializeField] GameObject m_stageStatusUI;
    /// <summary>�X�e�[�W�ԍ���\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI m_stageNumberText;
    /// <summary>�e�X�e�[�W�̖ڕW�œ_��\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI m_targetScoreText;
    /// <summary>�e�X�e�[�W�Ńv���C���[���l�������_��\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI m_getScoreText;
    /// <summary>�e�X�e�[�W�̃s�b�`���[�̋����̎c���\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI m_leftBallText;

    [Header("Result")]
    [SerializeField] GameObject m_resultUI;
    [SerializeField] TextMeshProUGUI m_resultText;

    [Header("�f�o�b�N�p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI m_displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUGUI();

        SubscribeEvents();

        //�^�C�g����BGM
        AudioManager.Instance.PlayBGM(KindOfBGM.Title);
    }

    /// <summary>
    /// �e��C�x���g��o�^����֐�.
    /// Start�֐����ŌĂяo������.
    /// </summary>
    private void SubscribeEvents()
    {
        m_startButton.onClick.AddListener(() =>
        {
            StartGame();
            m_startButton.gameObject.SetActive(false);
        });

        GameFlowManager.Instance.OnCurrentStageChanged.Subscribe((stage) =>
        {
            if (GameFlowManager.Instance.IsLastStage)
            {
                m_stageNumberText.text = $"FinalStage";
            }
            else
            {
                m_stageNumberText.text = $"Stage: {stage.stageNumber + 1}";
            }
            m_targetScoreText.text = $"�ڕW: {stage.clearScore}";
        });
        GameFlowManager.Instance.OnGetScoreChanged.Subscribe((score) =>
        {
            m_getScoreText.text = $"���_: {score}";
        });
        GameFlowManager.Instance.OnLeftBallCountChanged.Subscribe((leftBallCount) =>
        {
            m_leftBallText.text = $"�c��: {leftBallCount}";
        });
    }


    /// <summary>
    /// UGUI�̏������֐�.
    /// �Q�[�����n�߂�g���K�[�ȊO�ƃ����L���O�\�ȊO�͑S�Ĕ�\���ɂ���
    /// </summary>
    public void InitializeUGUI()
    {
        //�Q�[���J�n�g���K�[��On�ɂ���
        m_startButton.gameObject.SetActive(true);

        //��L��UI�ȊO�͑S�Ĕ�\���ɂ���
        m_judgmentResultOfBall?.gameObject.SetActive(false);
        m_stageStatusUI?.gameObject.SetActive(false);
        m_resultUI?.SetActive(false);
    }

    /// <summary>
    /// �X�^�[�g�{�^������������Q�[���J�n����֐�.
    /// </summary>
    public void StartGame()
    {
        //�^�C�g��BGM���~�߂�
        AudioManager.Instance.StopBGM();

        //�X�e�[�W���UI��\������
        m_stageStatusUI?.gameObject.SetActive(true);

        //�Q�[���J�n��SFX
        AudioManager.Instance.PlaySFX(KindOfSFX.PlayBall,
            () =>
            {
                //SFX�I����̃R�[���o�b�N
                //�X�e�[�W�P�J�n�I
                GameFlowManager.Instance.PlayBall(true, true);
                AudioManager.Instance.PlayBGM(KindOfBGM.InGame);
            });

    }

    /// <summary>
    /// �A�v���P�[�V�������I������֐�.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }

    /// <summary>
    /// WorldSpace���UI�Ƀ��b�Z�[�W��\������֐�.
    /// ex�F�q�b�g����A�Q�[���I�[�o�[�A�Q�[���N���Aetc...
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callBack"></param>
    public async void DisplayMessage(string message, UnityAction callBack = null)
    {
        if (m_judgmentResultOfBall)
        {
            m_judgmentResultOfBall.gameObject.SetActive(true);
            m_judgmentResultOfBall.text = message;

            await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);

            m_judgmentResultOfBall.gameObject.SetActive(false);
        }
        callBack?.Invoke();
    }

    /// <summary>
    /// WorldSpace��ɃX�e�[�W�N���A���̃��U���g��\������֐�.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="callBack"></param>
    public async void DisplayResult(Result result, UnityAction callBack = null)
    {
        m_resultUI?.SetActive(true);

        m_resultText.text = $"�q�b�g: {result.hitCount}�{\n" +
            $"�c�[����: {result.twoBaseCount}�{\n" +
            $"�X���[����: {result.threeBaseCount}�{\n" +
            $"�z�[������: {result.homeRunCount}�{\n" +
            $"�X�g���C�N: {result.strikeCount}��\n" +
            $"�ő勗��: {result.maxDistance}m\n" +
            $"���v����: {result.sumDistance}m\n" +
            $"��V���z: {result.amountOfRemuneration}�~\n" +
            $"�݌v��V���z: {result.accumulatedRemuneration}�~\n";

        await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);
        m_resultUI?.SetActive(false);
        callBack?.Invoke();
    }

    /// <summary>
    /// �o�b�^�[�̃X�C���O�X�s�[�h��\������֐�.
    /// </summary>
    /// <param name="velo"></param>
    public void DisplayHeadSpeed(float velo)
    {
        if (velo < 70f) return;
        velo = Mathf.Floor(velo);

        if (m_displayHeadSpeedOfBatText)
        {
            m_displayHeadSpeedOfBatText.text = $"Head Speed\n{velo} km";
        }
    }
}
