using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junjun
{
    /// <summary>
    /// �{�[���̋���
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {
        /// <summary>�{�[���𓊂����ވʒu</summary>
        [SerializeField] GameObject m_catcherPos;

        /// <summary>�{�[���̃X�s�[�h</summary>
        [SerializeField] float m_speed;

        Rigidbody m_rb;

        /// <summary>
        /// �Ă΂ꂽ�u�ԂɃ~�b�g�߂����Ĕ��ł���
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