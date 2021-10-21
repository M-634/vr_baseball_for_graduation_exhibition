using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junjun
{
    /// <summary>
    /// �s�b�`���[�̋���
    /// </summary>
    public class Pitcher : MonoBehaviour
    {
        /// <summary>�s�b�`���[�̃A�j���[�V����</summary>
        [SerializeField] Animator m_anim;
        /// <summary>�{�[���̃I�u�W�F�N�g</summary>
        [SerializeField] GameObject m_ball;
        /// <summary>�{�[���𓊂���ʒu</summary>
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