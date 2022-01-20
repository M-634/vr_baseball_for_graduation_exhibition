using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;

/// <summary>
/// WorldSpace上のUI制御するクラス
/// </summary>
public class UGUIControl : MonoBehaviour
{
    /// <summary>球の判定結果を表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI m_judgmentResultOfBall = default;

    /// <summary>スタートボタン</summary>
    [SerializeField] Button m_startButton;

    [Header("デバック用テキスト")]
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
    /// スタートボタンを押したらゲーム開始する関数.
    /// </summary>
    public void StartGame()
    {
        //ステージ１開始！
        GameFlowManager.Instance.PlayBall();
    }

    /// <summary>
    /// デバック用関数：Oculus上で実行中のアプリケーションを終了する.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        Application.Quit();
#endif
    }

    /// <summary>
    /// ピッチャーがボールを投げた後の球の判定を表示する関数.
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
    /// バッターのスイングスピードを表示する関数.
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
