using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junjun
{
    /// <summary>
    /// ボールの挙動
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        /// <summary>ボールを投げ込む位置</summary>
        [SerializeField] GameObject m_catcherPos;

        /// <summary>ボールのスピード</summary>
        [SerializeField] float m_speed;

        Rigidbody m_rb;

        /// <summary>
        /// 呼ばれた瞬間にミットめがけて飛んでいく
        /// </summary>
        void BallMove()
        {
            m_rb.velocity = (m_catcherPos.transform.position - transform.position) * m_speed;
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

            BallMove();
        }
    }
}