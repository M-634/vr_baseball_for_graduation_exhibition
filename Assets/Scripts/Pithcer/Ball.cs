using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �{�[���̋���
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    /// <summary>�{�[���𓊂����ވʒu</summary>
    [SerializeField] GameObject m_catcherPos;

    /// <summary>����</summary>
    [SerializeField] public BallType m_ballType;
    /// <summary>�{�[���̃X�s�[�h</summary>
    [SerializeField] float m_speed;

    [SerializeField] float m_changePower = 100;

    /// <summary>�X�g���[�g�̗�</summary>
    [SerializeField] Vector3 m_straightDirection = new Vector3(0f, 0.2f, 1.0f);

    [SerializeField] Vector3 m_curveDirection;

    [SerializeField] Vector3 m_sliderDirection = new Vector3(1.0f, -1.0f, 0f);

    Rigidbody m_rb;

    /// <summary>
    /// �Ă΂ꂽ�u�ԂɃ~�b�g�߂����Ĕ��ł���
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
                break;
            case BallType.Slider:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(0.6f);
                m_rb.AddForceAtPosition(m_sliderDirection * m_changePower, m_catcherPos.transform.position);
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// �����ς���֐�
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

        StartCoroutine(BallMove());
    }
}

public enum BallType
{
    Straight = 0,
    Curve = 1,
    Slider = 2
}
