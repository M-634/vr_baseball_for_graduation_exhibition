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
        /// <summary>�����Q�[��������̒e������</summary>
        [SerializeField] int m_ballLimit = 30;
        /// <summary>�{�[���𓊂���ʒu</summary>
        [SerializeField] GameObject m_throwPos;

        public void Throw()
        {
            if (m_ballLimit != 0)
            {
                m_ballLimit -= 1;
                m_ball.transform.position = m_throwPos.transform.position;
                m_ball.gameObject.SetActive(true);
            }
        }
    }
}