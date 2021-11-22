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
    [SerializeField] TextMeshProUGUI displayJudgeText = default;

    // Start is called before the first frame update
    void Start()
    {
        displayJudgeText.gameObject.SetActive(false);

        BaseBallLogic.Instance.OnSendProcessMessage += DisplayMessage;
    }

    private async UniTask DisplayMessage(JudgeType judgeType)
    {
        if (judgeType == JudgeType.None)
        {
            Debug.Log("�����\�����Ȃ�");
            return;
        }

        if (displayJudgeText)
        {
            displayJudgeText.gameObject.SetActive(true);
            displayJudgeText.text = judgeType.ToString();
        }

        //���菈�����o���e�L�X�g�̕\�����I�������^�X�N�I��
        await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);
        displayJudgeText.gameObject.SetActive(false);
        Debug.Log("end UGUI task...");
    }
}
