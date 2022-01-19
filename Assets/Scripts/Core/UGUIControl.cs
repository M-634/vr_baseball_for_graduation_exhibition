using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// WorldSpace���UI���䂷��N���X
/// </summary>
public class UGUIControl : MonoBehaviour
{
    /// <summary>���̔��茋�ʂ�\������e�L�X�g</summary>
    [SerializeField] TextMeshProUGUI judgmentResultOfBall = default;

    [Header("�f�o�b�N�p�e�L�X�g")]
    [SerializeField] TextMeshProUGUI displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        judgmentResultOfBall.gameObject.SetActive(false);
    }


    /// <summary>
    /// �s�b�`���[���{�[���𓊂�����̋��̔����\������֐�.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callBack"></param>
    public async void DisplayHitZoneMessage(string message,UnityAction callBack = null)
    {
        if (judgmentResultOfBall)
        {
            judgmentResultOfBall.gameObject.SetActive(true);
            judgmentResultOfBall.text = message;

            await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);

            judgmentResultOfBall.gameObject.SetActive(false);
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

        if (displayHeadSpeedOfBatText)
        {
            displayHeadSpeedOfBatText.text = $"Head Speed\n{velo} km";
        }
    }
}
