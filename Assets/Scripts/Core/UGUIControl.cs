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
    [SerializeField] Button m_startButton = default;

    [Header("StageSatus")]
    /// <summary>ゲーム中の各ステージのステータスをUIに表示するオブジェット</summary>
    [SerializeField] GameObject m_stageStatusUI;
    /// <summary>ステージ番号を表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI m_stageNumberText;
    /// <summary>各ステージの目標打点を表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI m_targetScoreText;
    /// <summary>各ステージでプレイヤーが獲得した点を表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI m_getScoreText;
    /// <summary>各ステージのピッチャーの球数の残りを表示するテキスト</summary>
    [SerializeField] TextMeshProUGUI m_leftBallText;

    [Header("Result")]
    [SerializeField] GameObject m_resultUI;
    [SerializeField] TextMeshProUGUI m_resultText;

    [Header("デバック用テキスト")]
    [SerializeField] TextMeshProUGUI m_displayHeadSpeedOfBatText = default;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUGUI();

        SubscribeEvents();

        //タイトルのBGM
        AudioManager.Instance.PlayBGM(KindOfBGM.Title);
    }

    /// <summary>
    /// 各種イベントを登録する関数.
    /// Start関数内で呼び出すこと.
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
            m_targetScoreText.text = $"目標: {stage.clearScore}";
        });
        GameFlowManager.Instance.OnGetScoreChanged.Subscribe((score) =>
        {
            m_getScoreText.text = $"得点: {score}";
        });
        GameFlowManager.Instance.OnLeftBallCountChanged.Subscribe((leftBallCount) =>
        {
            m_leftBallText.text = $"残り: {leftBallCount}";
        });
    }


    /// <summary>
    /// UGUIの初期化関数.
    /// ゲームを始めるトリガー以外とランキング表以外は全て非表示にする
    /// </summary>
    public void InitializeUGUI()
    {
        //ゲーム開始トリガーをOnにする
        m_startButton.gameObject.SetActive(true);

        //上記のUI以外は全て非表示にする
        m_judgmentResultOfBall?.gameObject.SetActive(false);
        m_stageStatusUI?.gameObject.SetActive(false);
        m_resultUI?.SetActive(false);
    }

    /// <summary>
    /// スタートボタンを押したらゲーム開始する関数.
    /// </summary>
    public void StartGame()
    {
        //タイトルBGMを止める
        AudioManager.Instance.StopBGM();

        //ステージ情報UIを表示する
        m_stageStatusUI?.gameObject.SetActive(true);

        //ゲーム開始のSFX
        AudioManager.Instance.PlaySFX(KindOfSFX.PlayBall,
            () =>
            {
                //SFX終了後のコールバック
                //ステージ１開始！
                GameFlowManager.Instance.PlayBall(true, true);
                AudioManager.Instance.PlayBGM(KindOfBGM.InGame);
            });

    }

    /// <summary>
    /// アプリケーションを終了する関数.
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
    /// WorldSpace上のUIにメッセージを表示する関数.
    /// ex：ヒット判定、ゲームオーバー、ゲームクリアetc...
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
    /// WorldSpace上にステージクリア時のリザルトを表示する関数.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="callBack"></param>
    public async void DisplayResult(Result result, UnityAction callBack = null)
    {
        m_resultUI?.SetActive(true);

        m_resultText.text = $"ヒット: {result.hitCount}本\n" +
            $"ツーラン: {result.twoBaseCount}本\n" +
            $"スリーラン: {result.threeBaseCount}本\n" +
            $"ホームラン: {result.homeRunCount}本\n" +
            $"ストライク: {result.strikeCount}数\n" +
            $"最大距離: {result.maxDistance}m\n" +
            $"合計距離: {result.sumDistance}m\n" +
            $"報酬金額: {result.amountOfRemuneration}円\n" +
            $"累計報酬金額: {result.accumulatedRemuneration}円\n";

        await UniTask.Delay(System.TimeSpan.FromSeconds(2f), ignoreTimeScale: false);
        m_resultUI?.SetActive(false);
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
