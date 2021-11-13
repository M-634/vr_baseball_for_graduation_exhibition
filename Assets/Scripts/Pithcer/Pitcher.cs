using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ピッチャーの挙動
/// </summary>
public class Pitcher : MonoBehaviour
{
    /// <summary>ピッチャーのアニメーション</summary>
    [SerializeField] Animator m_anim;

    /// <summary>ボールのオブジェクト</summary>
    [SerializeField] Ball m_ball;
    /// <summary>ワンゲーム当たりの弾数制限</summary>
    [SerializeField] int m_ballLimit = 30;
    /// <summary>ボールを投げる位置</summary>
    [SerializeField] GameObject m_throwPos;

    /// <summary>投げる間隔</summary>
    [SerializeField] float m_throwIntervalTime = 3;
    /// <summary>球種番号</summary>
    int m_ballType;

    [SerializeField, Header("ここをDebugで永遠ストレートの刑")] DevelopType m_type;

    private void Start()
    {
        if (m_type == DevelopType.Debug)
        {
            m_ballLimit = 9999;
            PitcherUI.Instance.m_currentBallNum.text = "残りの球数 : " + m_ballLimit.ToString();
        }
        m_ball = m_ball.GetComponent<Ball>();
        //m_ball.OnThrowAction += () => ThrowBall();
        //ThrowBall();
    }

    /// <summary>
    /// コールバックで呼ばれる関数
    /// </summary>
    public void ThrowBall()
    {
        if (m_ballLimit == 0)
        {
            return;
        }

        if (m_type == DevelopType.Debug)
        {
            m_ballType = 0;
        }
        else
        {
            m_ballType = Random.Range(0, 10);
        }

        // アニメーションのせいで位置がずれまくるので補正
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(0, 0, 0);

        m_ball.ChangeBallType(m_ballType);
        m_anim.SetTrigger("Throw");
    }

    /// <summary>
    /// アニメーションイベントで呼ぶ
    /// </summary>
    public void Throw()
    {
        m_ballLimit--;
        PitcherUI.Instance.m_currentBallNum.text = "残りの球数 : " + m_ballLimit.ToString();
        m_ball.gameObject.SetActive(true);
    }
}

/// <summary>
/// 開発タイプ
/// </summary>
public enum DevelopType
{
    Debug,
    Main
}