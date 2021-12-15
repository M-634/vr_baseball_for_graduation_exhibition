using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


/// <summary>
/// ボールの挙動
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    /// <summary>ボールを投げ込む位置</summary>
    [SerializeField] GameObject m_catcherPos;
    /// <summary>キャッチャーの初期化処理</summary>
    Vector3 m_initCatcherPos;
    /// <summary>ボールが消える瞬間に出るエフェクト</summary>
    [SerializeField] GameObject m_arrivalPoint;

    /// <summary>球種</summary>
    [SerializeField] public BallType m_ballType;
    /// <summary>ボールに加える力</summary>
    [SerializeField] float m_force = 1000;
    /// <summary>変化させるときの力</summary>
    [SerializeField] float m_changePower = 80;
    /// <summary>変化させるタイミング</summary>
    [SerializeField] float m_changeTime = 0.6f;

    /// <summary>ボールをスピード</summary>
    float m_speed;

    /// <summary>ボールを投げる位置</summary>
    [SerializeField] GameObject m_throwPos;
    /// <summary>スピードの計測位置</summary>
    [SerializeField] GameObject m_speedGun;

    /// <summary>Rayを飛ばす距離（1fでいい感じ） </summary>
    [SerializeField] float hitDistance = 1f;
    /// <summary>到達時間</summary>
    float m_time;

    Vector3 m_previousPos;

    /// <summary>バットに当たったかどうか</summary>
    bool onHitBat;

    #region Ball Type Direction
    /// <summary>ストレートの力の方向</summary>
    [SerializeField] Vector3 m_straightDirection = new Vector3(0f, 0.2f, 1.0f);
    /// <summary>高速ストレートの力の方向</summary>
    [SerializeField] Vector3 m_highSpeedStraightDirection = new Vector3(0f, 0.1f, 3.0f);
    /// <summary>カーブの最初の力の方向</summary>
    [SerializeField] Vector3 m_curveDirection = new Vector3(0.18f, 0.35f, 0f);
    /// <summary>カーブの変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_changeCurveDirection = new Vector3(-9f, -7f, 0.2f);
    /// <summary>スライダー変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_sliderDirection = new Vector3(-1.0f, -0.8f, 0f);
    /// <summary>シュート変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_shootDirection = new Vector3(1.0f, 0f, 0f);
    /// <summary>フォーク変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_forkDirection = new Vector3(0f, -1.5f, 0f);
    /// <summary>シンカー変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_sinkerDirection = new Vector3(1.0f, -0.8f, 0f);
    /// <summary>チェンジアップ変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_changeUpDirection = new Vector3(0f, -0.1f, -1.5f);
    /// <summary>ライズボール変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_rizeBallDirection = new Vector3(0f, 0.8f, 3f);
    /// <summary>カットボール変化させるときに加える力の方向</summary>
    [SerializeField] Vector3 m_cutBallDirection = new Vector3(-0.8f, 0f, 0f);

    bool m_isCurve = false;

    #endregion

    #region Magic Ball
    /// <summary>魔球かどうか</summary>
    bool m_isMagicBall = false;

    /// <summary>魔球のスピード</summary>
    [SerializeField] float m_mBSpeed;

    #endregion

    /// <summary>投げている球種を表示するテキスト</summary>
    [SerializeField] Text m_ballTypeText;

    [SerializeField] Rigidbody m_rb;

    //float m_hitTime;

    //public event Action OnThrowAction = default;

    /// <summary>ボールの軌跡</summary>
    [SerializeField] TrailRenderer m_ballTrail;

    private void Start()
    {
        m_initCatcherPos = new Vector3(0, 0, 21.5f);
    }

    private void Update()
    {
        if (!onHitBat) return;

        if (onHitBat)
        {
            m_rb.useGravity = true;
            if (m_rb.velocity.magnitude <= 0.3f)
            {
                m_arrivalPoint.transform.position = transform.position;
                m_arrivalPoint.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (m_isCurve)
        {
            m_rb.AddForceAtPosition(m_changeCurveDirection * 1, m_catcherPos.transform.position);
        }

        if (m_ballType == BallType.WhiteBall)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time * 100f) * 0.5f, transform.position.z);

            m_rb.velocity = m_catcherPos.transform.position * m_mBSpeed;
        }
        else if (m_ballType == BallType.WanderWhiteBall)
        {
            transform.position = new Vector3(Mathf.Sin(Time.time * 30f), transform.position.y, transform.position.z);
            m_rb.velocity = m_catcherPos.transform.position * m_mBSpeed;
        }

        HitCheck();
    }

    /// <summary>
    /// マイフレーム(1/90f)ごとにRayを飛ばして当たり判定をする関数
    /// </summary>
    private void HitCheck()
    {
        if (Physics.Raycast(transform.position, transform.position - m_previousPos, out RaycastHit hit, hitDistance))
        {
            if (hit.collider.TryGetComponent(out IBallHitObjet obj))
            {
                obj.OnHit(m_rb, hit, m_speed);
            }

            if (hit.collider.gameObject.CompareTag("BatMesh"))
            {
                onHitBat = true;
               
                Debug.Log("Hit bat");
            }
            //m_hitTime = Time.time;
        }

        Debug.DrawLine(transform.position, transform.position + (transform.position - m_previousPos) * hitDistance, Color.red);
        m_previousPos = transform.position;
    }

    /// <summary>
    /// 呼ばれた瞬間にミットめがけて飛んでいく
    /// </summary>
    IEnumerator BallMove()
    {
        m_rb.velocity = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        m_straightDirection = new Vector3(0, PitcherUI.Instance.m_heightAdjust.value * 0.025f + 0.2f, m_straightDirection.z);

        switch (m_ballType)
        {
            case BallType.Straight:
                m_ballTypeText.text = "ストレート";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                break;
            case BallType.Curve:
                m_ballTypeText.text = "カーブ";
                m_curveDirection = new Vector3(0.18f, PitcherUI.Instance.m_heightAdjust.value * 0.02f + 0.36f, 1.0f);
                m_rb.AddForceAtPosition(m_curveDirection * m_force, m_catcherPos.transform.position);
                m_isCurve = true;
                break;
            case BallType.Slider:
                m_ballTypeText.text = "スライダー";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sliderDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Shoot:
                m_ballTypeText.text = "シュート";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_shootDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Fork:
                m_ballTypeText.text = "フォーク";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_forkDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Sinker:
                m_ballTypeText.text = "シンカー";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sinkerDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.ChangeUp:
                m_ballTypeText.text = "チェンジアップ";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                m_changeTime = 0.2f;
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_changeUpDirection * m_changePower, m_catcherPos.transform.position);
                m_changeTime = 0.6f;
                break;
            case BallType.HighSpeedStraight:
                m_ballTypeText.text = "速いストレート";
                m_highSpeedStraightDirection = new Vector3(0, PitcherUI.Instance.m_heightAdjust.value * 0.03f + 0.12f, 1.5f);
                m_rb.AddForceAtPosition(m_highSpeedStraightDirection * m_force, m_catcherPos.transform.position);
                break;
            case BallType.RizeBall:
                m_ballTypeText.text = "ライズボール";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_rizeBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.CutBall:
                m_ballTypeText.text = "カットボール";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_cutBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.WhiteBall:
                m_ballTypeText.text = "ホワイトボール";

                break;
            case BallType.WanderWhiteBall:
                m_ballTypeText.text = "ワンダーホワイトボール";

                break;

            case BallType.DragonflyBall:
                m_ballTypeText.text = "トンボール";
                m_rb.velocity = m_catcherPos.transform.position * m_mBSpeed;
                yield return new WaitForSeconds(m_changeTime);
                m_rb.velocity = Vector3.zero;
                yield return new WaitForSeconds(m_changeTime);
                m_rb.velocity = m_catcherPos.transform.position * m_mBSpeed;
                break;
            default:
                break;
        }
    }

    IEnumerator Timer()
    {
        m_time = 0f;
        while (true)
        {
            m_time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// 球種を変える関数
    /// </summary>
    /// <param name="ballType"></param>
    public void ChangeBallType(int ballType)
    {
        if (ballType >= 10)
        {
            m_isMagicBall = true;

            m_rb.useGravity = false;
        }
        m_ballType = (BallType)ballType;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "SpeedGun")
        {
            StopCoroutine(Timer());
            m_speed = (m_speedGun.transform.position.z - m_throwPos.transform.position.z) / m_time;
            m_speed *= 3.6f;

            Debug.Log(Mathf.Floor(m_speed) + "km");
        }
    }

    private void OnEnable()
    {
        transform.position = m_throwPos.transform.position;
        m_isCurve = false;
        StartCoroutine(BallMove());
        StartCoroutine(Timer());
        if (m_ballType == BallType.WhiteBall)
        {
            m_catcherPos.transform.position = new Vector3(m_catcherPos.transform.position.x, m_catcherPos.transform.position.y + 100, m_catcherPos.transform.position.z);
        }
    }

    private void OnDisable()
    {
        m_catcherPos.transform.position = m_initCatcherPos;
        m_ballTrail.Clear();
        BaseBallLogic.Instance.EndMoveBall();
        onHitBat = false;
        m_rb.useGravity = true;
    }
}

public enum BallType
{
    Straight = 0,
    Curve = 1,
    Slider = 2,
    Shoot = 3,
    Fork = 4,
    Sinker = 5,
    ChangeUp = 6,
    HighSpeedStraight = 7,
    RizeBall = 8,
    CutBall = 9,
    WhiteBall = 10,
    WanderWhiteBall = 11,
    DragonflyBall = 12
}
