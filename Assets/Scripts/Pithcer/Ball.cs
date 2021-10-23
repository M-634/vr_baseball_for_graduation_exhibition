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
    /// <summary>�ω�������Ƃ��̗�</summary>
    [SerializeField] float m_changePower = 100;
    /// <summary>�ω�������^�C�~���O</summary>
    [SerializeField] float m_changeTime = 0.6f;

    /// <summary>�X�g���[�g�̗͂̕���</summary>
    [SerializeField] Vector3 m_straightDirection = new Vector3(0f, 0.2f, 1.0f);
    /// <summary>�J�[�u�̍ŏ��̗͂̕���</summary>
    [SerializeField] Vector3 m_curveDirection = new Vector3(0.05f, 0.25f, 0f);
    /// <summary>�J�[�u�̕ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_changeCurveDirection = new Vector3(-0.1f, -1.0f, 0f);
    /// <summary>�X���C�_�[�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_sliderDirection = new Vector3(-1.0f, -0.8f, 0f);
    /// <summary>�V���[�g�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_shootDirection = new Vector3(1.0f, 0f, 0f);
    /// <summary>�t�H�[�N�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_forkDirection = new Vector3(0f, -1.5f, 0f);
    /// <summary>�V���J�[�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_sinkerDirection = new Vector3(1.0f, -0.8f, 0f);

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
                m_rb.AddForceAtPosition(m_curveDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_changeCurveDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Slider:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sliderDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Shoot:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_shootDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Fork:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_forkDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Sinker:
                m_rb.AddForceAtPosition(m_straightDirection * m_speed, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sinkerDirection * m_changePower, m_catcherPos.transform.position);
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
    Slider = 2,
    Shoot = 3,
    Fork = 4,
    Sinker = 5
}
