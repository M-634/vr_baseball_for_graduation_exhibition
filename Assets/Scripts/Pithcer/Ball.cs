using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ボールの挙動
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    /// <summary>ボールを投げ込む位置</summary>
    [SerializeField] GameObject m_catcherPos;

    [SerializeField] GameObject m_throwPos;

    /// <summary>球種</summary>
    [SerializeField] public BallType m_ballType;
    /// <summary>ボールのスピード</summary>
    [SerializeField] float m_speed;
    /// <summary>変化させるときの力</summary>
    [SerializeField] float m_changePower = 100;
    /// <summary>変化させるタイミング</summary>
    [SerializeField] float m_changeTime = 0.6f;

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

    Rigidbody m_rb;

    private void FixedUpdate()
    {
        if (m_isCurve)
        {
            m_rb.AddForceAtPosition(m_changeCurveDirection * 1, m_catcherPos.transform.position);
        }
    }

    /// <summary>
    /// 呼ばれた瞬間にミットめがけて飛んでいく
    /// </summary>
    IEnumerator BallMove()
    {
        m_rb.velocity = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        switch (m_ballType)
        {
            case BallType.Straight:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                break;
            case BallType.Curve:
                m_rb.AddForceAtPosition(m_curveDirection * m_speed, m_catcherPos.transform.position);
                m_isCurve = true;
                break;
            case BallType.Slider:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sliderDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Shoot:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_shootDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Fork:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_forkDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Sinker:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sinkerDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.ChangeUp:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                m_changeTime = 0.2f;
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_changeUpDirection * m_changePower, m_catcherPos.transform.position);
                m_changeTime = 0.6f;
                break;
            case BallType.HighSpeedStraight:
                m_rb.AddForceAtPosition(m_highSpeedStraightDirection * m_speed, m_catcherPos.transform.position);
                break;
            case BallType.RizeBall:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_rizeBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.CutBall:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_cutBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 球種を変える関数
    /// </summary>
    /// <param name="ballType"></param>
    public void ChangeBallType(int ballType)
    {
        m_ballType = (BallType)ballType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Catcher")
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (m_rb == null)
        {
            m_rb = GetComponent<Rigidbody>();
        }

        m_isCurve = false;
        StartCoroutine(BallMove());
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
    CutBall = 9
}
