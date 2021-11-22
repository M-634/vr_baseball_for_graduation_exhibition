using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// WorldSpace上のUI制御するクラス
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
            Debug.Log("何も表示しない");
            return;
        }

        if (displayJudgeText)
        {
            displayJudgeText.gameObject.SetActive(true);
            displayJudgeText.text = judgeType.ToString();
        }

        //判定処理を出すテキストの表示が終わったらタスク終了
        await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);
        displayJudgeText.gameObject.SetActive(false);
        Debug.Log("end UGUI task...");
    }
}
