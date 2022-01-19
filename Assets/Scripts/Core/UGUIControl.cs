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
    /// <summary>球の判定結果を表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI judgmentResultOfBall = default;

    [Header("デバック用テキスト")]
    [SerializeField] TextMeshProUGUI displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        judgmentResultOfBall.gameObject.SetActive(false);
    }


    /// <summary>
    /// ピッチャーがボールを投げた後の球の判定を表示する関数.
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
    /// バッターのスイングスピードを表示する関数.
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
