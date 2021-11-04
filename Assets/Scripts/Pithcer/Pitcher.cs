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
    [SerializeField] GameObject m_ball;
    /// <summary>ワンゲーム当たりの弾数制限</summary>
    [SerializeField] int m_ballLimit = 30;
    /// <summary>ボールを投げる位置</summary>
    [SerializeField] GameObject m_throwPos;

    /// <summary>
    /// アニメーションイベントで呼ぶ
    /// </summary>
    public void Throw()
    {
        if (m_ballLimit != 0)
        {
            m_ballLimit -= 1;
            PitcherUI.Instance.m_currentBallNum.text = m_ballLimit.ToString();
            m_ball.transform.position = m_throwPos.transform.position;
            m_ball.gameObject.SetActive(true);
        }
    }
}
