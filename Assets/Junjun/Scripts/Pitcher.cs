using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junjun
{
    /// <summary>
    /// ピッチャーの挙動
    /// </summary>
    public class Pitcher : MonoBehaviour
    {
        /// <summary>ピッチャーのアニメーション</summary>
        [SerializeField] Animator m_anim;
        /// <summary>ボールのオブジェクト</summary>
        [SerializeField] GameObject m_ball;
        /// <summary>ボールを投げる位置</summary>
        [SerializeField] GameObject m_throwPos;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_anim.SetTrigger("Throw");
            }
        }

        public void Throw()
        {
            m_ball.transform.position = m_throwPos.transform.position;
            m_ball.gameObject.SetActive(true);
        }
    }
}