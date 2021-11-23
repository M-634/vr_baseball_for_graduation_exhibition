using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VR_BaseBall.Test
{
    public class TestBall : MonoBehaviour
    {
        /// <summary>ボールスピード（max 200まで）</summary>
        [SerializeField] float m_speed = 5f;
        /// <summary>Rayを飛ばす距離（1fでいい感じ） </summary>
        [SerializeField] float m_hitDistance = 1f;
        Rigidbody m_rb;

        Vector3 m_previousPos;
        float m_velo;
        bool m_onHit;

        // Start is called before the first frame update
        void Start()
        {
            m_previousPos = transform.position;
            m_rb = GetComponent<Rigidbody>();
            //m_rb.velocity = Vector3.forward * m_speed;
        }

        /// <summary>
        /// FixedUpdateが呼ばれる回数 Fixed Timestep = 1/90 で調整しています。
        /// </summary>
        private void FixedUpdate()
        {
            //if (onHit) return;
            var temp = transform.position - m_previousPos;
            m_velo = temp.magnitude / Time.fixedDeltaTime * 3.6f;
            HitCheck();
        }


        /// <summary>
        /// マイフレーム(1/90f)ごとにRayを飛ばして当たり判定をする関数
        /// </summary>
        private void HitCheck()
        {
            if (Physics.Raycast(transform.position, transform.position - m_previousPos, out RaycastHit hit, m_hitDistance))
            {
                if (hit.collider.TryGetComponent(out IBallHitObjet obj))
                {
                    obj.OnHit(m_rb, hit, m_velo);
                    m_onHit = true;
                }
            }

            Debug.DrawLine(transform.position, transform.position + (transform.position - m_previousPos) * m_hitDistance, Color.red);
            m_previousPos = transform.position;
        }
    }
}
