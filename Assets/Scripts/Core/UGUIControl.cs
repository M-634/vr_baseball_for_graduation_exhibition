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
    [SerializeField] Button m_startButton;

    [Header("�f�o�b�N�p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI m_displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        m_judgmentResultOfBall?.gameObject.SetActive(false);

        m_startButton.onClick.AddListener(() =>
            {
                StartGame();
                m_startButton.gameObject.SetActive(false);
            });
    }
   
    /// <summary>
    /// �X�^�[�g�{�^������������Q�[���J�n����֐�.
    /// </summary>
    public void StartGame()
    {
        //�X�e�[�W�P�J�n�I
        GameFlowManager.Instance.PlayBall();
    }

    /// <summary>
    /// �f�o�b�N�p�֐��FOculus��Ŏ��s���̃A�v���P�[�V�������I������.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        Application.Quit();
#endif
    }

    /// <summary>
    /// �s�b�`���[���{�[���𓊂�����̋��̔����\������֐�.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callBack"></param>
    public async void DisplayHitZoneMessage(string message,UnityAction callBack = null)
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
