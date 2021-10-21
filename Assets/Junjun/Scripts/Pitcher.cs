using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junjun
{
    public class Pitcher : MonoBehaviour
    {
        /// <summary>�s�b�`���[�̃A�j���[�V����</summary>
        [SerializeField] Animator m_anim;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_anim.SetTrigger("Throw");
            }
        }
    }
}