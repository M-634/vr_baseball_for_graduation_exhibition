using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


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
    /// <summary>�{�[���ɉ������</summary>
    [SerializeField] float m_force = 1000;
    /// <summary>�ω�������Ƃ��̗�</summary>
    [SerializeField] float m_changePower = 80;
    /// <summary>�ω�������^�C�~���O</summary>
    [SerializeField] float m_changeTime = 0.6f;

    // �{�[���̑��x
    float m_speed;

    [SerializeField] GameObject m_throwPos;
    /// <summary>�X�s�[�h�̌v���ʒu</summary>
    [SerializeField] GameObject m_speedGun;
    /// <summary>���B����</summary>
    float m_time;

    #region Ball Type Direction
    /// <summary>�X�g���[�g�̗͂̕���</summary>
    [SerializeField] Vector3 m_straightDirection = new Vector3(0f, 0.2f, 1.0f);
    /// <summary>�����X�g���[�g�̗͂̕���</summary>
    [SerializeField] Vector3 m_highSpeedStraightDirection = new Vector3(0f, 0.1f, 3.0f);
    /// <summary>�J�[�u�̍ŏ��̗͂̕���</summary>
    [SerializeField] Vector3 m_curveDirection = new Vector3(0.18f, 0.35f, 0f);
    /// <summary>�J�[�u�̕ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_changeCurveDirection = new Vector3(-9f, -7f, 0.2f);
    /// <summary>�X���C�_�[�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_sliderDirection = new Vector3(-1.0f, -0.8f, 0f);
    /// <summary>�V���[�g�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_shootDirection = new Vector3(1.0f, 0f, 0f);
    /// <summary>�t�H�[�N�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_forkDirection = new Vector3(0f, -1.5f, 0f);
    /// <summary>�V���J�[�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_sinkerDirection = new Vector3(1.0f, -0.8f, 0f);
    /// <summary>�`�F���W�A�b�v�ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_changeUpDirection = new Vector3(0f, -0.1f, -1.5f);
    /// <summary>���C�Y�{�[���ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_rizeBallDirection = new Vector3(0f, 0.8f, 3f);
    /// <summary>�J�b�g�{�[���ω�������Ƃ��ɉ�����͂̕���</summary>
    [SerializeField] Vector3 m_cutBallDirection = new Vector3(-0.8f, 0f, 0f);

    bool m_isCurve = false;

    #endregion

    /// <summary>�����Ă��鋅���\������e�L�X�g</summary>
    [SerializeField] Text m_ballTypeText;

    Rigidbody m_rb;

    private void FixedUpdate()
    {
        if (m_isCurve)
        {
            m_rb.AddForceAtPosition(m_changeCurveDirection * 1, m_catcherPos.transform.position);
        }
    }

    /// <summary>
    /// �Ă΂ꂽ�u�ԂɃ~�b�g�߂����Ĕ��ł���
    /// </summary>
    IEnumerator BallMove()
    {
        m_rb.velocity = Vector3.zero;
        transform.rotation = new Quaternion(0, 0, 0, 0);
        m_straightDirection = new Vector3(0, PitcherUI.Instance.m_heightAdjust.value * 0.025f + 0.2f, m_straightDirection.z);

        switch (m_ballType)
        {
            case BallType.Straight:
                m_ballTypeText.text = "�X�g���[�g";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                break;
            case BallType.Curve:
                m_ballTypeText.text = "�J�[�u";
                m_curveDirection = new Vector3(0.18f, PitcherUI.Instance.m_heightAdjust.value * 0.02f + 0.36f, 1.0f);
                m_rb.AddForceAtPosition(m_curveDirection * m_force, m_catcherPos.transform.position);
                m_isCurve = true;
                break;
            case BallType.Slider:
                m_ballTypeText.text = "�X���C�_�[";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sliderDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Shoot:
                m_ballTypeText.text = "�V���[�g";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_shootDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Fork:
                m_ballTypeText.text = "�t�H�[�N";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_forkDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.Sinker:
                m_ballTypeText.text = "�V���J�[";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_sinkerDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.ChangeUp:
                m_ballTypeText.text = "�`�F���W�A�b�v";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                m_changeTime = 0.2f;
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_changeUpDirection * m_changePower, m_catcherPos.transform.position);
                m_changeTime = 0.6f;
                break;
            case BallType.HighSpeedStraight:
                m_ballTypeText.text = "�����X�g���[�g";
                m_highSpeedStraightDirection = new Vector3(0, PitcherUI.Instance.m_heightAdjust.value * 0.03f + 0.12f, 1.5f);
                m_rb.AddForceAtPosition(m_highSpeedStraightDirection * m_force, m_catcherPos.transform.position);
                break;
            case BallType.RizeBall:
                m_ballTypeText.text = "���C�Y�{�[��";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_rizeBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            case BallType.CutBall:
                m_ballTypeText.text = "�J�b�g�{�[��";
                m_rb.AddForceAtPosition(m_straightDirection * m_force, m_catcherPos.transform.position);
                yield return new WaitForSeconds(m_changeTime);
                m_rb.AddForceAtPosition(m_cutBallDirection * m_changePower, m_catcherPos.transform.position);
                break;
            default:
                break;
        }
    }

    IEnumerator Timer()
    {
        m_time = 0f;
        while (true)
        {
            m_time += Time.unscaledDeltaTime;
            yield return null;
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

        if (other.gameObject.tag == "SpeedGun")
        {
            StopCoroutine(Timer());
            m_speed = (m_speedGun.transform.position.z - m_throwPos.transform.position.z) / m_time;
            m_speed *= 3.6f;

            Debug.Log(Mathf.Floor(m_speed) + "km");
        }
    }

    private void OnEnable()
    {
        if (m_rb == null)
        {
            m_rb = GetComponent<Rigidbody>();
        }

        m_isCurve = false;
        StartCoroutine(BallMove());
        StartCoroutine(Timer());
    }
}

public enum BallType
{
    Straight = 0,
    Curve = 1,
    Slider = 2,
    Shoot = 3,
    Fork = 4,
    Sinker = 5,
    ChangeUp = 6,
    HighSpeedStraight = 7,
    RizeBall = 8,
    CutBall = 9
}
