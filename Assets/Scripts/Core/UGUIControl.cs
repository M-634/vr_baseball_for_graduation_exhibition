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

    [Header("デバック用テキスト")]
    [SerializeField] TextMeshProUGUI displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        displayJudgeText.gameObject.SetActive(false);

        GameFlowManager.Instance.OnSendProcessMessage += DisplayMessage;
    }

    private async void DisplayMessage(string message,UnityAction callBack = null)
    {
        if (displayJudgeText)
        {
            displayJudgeText.gameObject.SetActive(true);
            displayJudgeText.text = message;
        }
        await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);
        displayJudgeText.gameObject.SetActive(false);

        callBack?.Invoke();
    }


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
