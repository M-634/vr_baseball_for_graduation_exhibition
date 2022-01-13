using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �s�b�`���[�̋���
/// </summary>
public class Pitcher : MonoBehaviour
{

    /// <summary>�s�b�`���[�̃A�j���[�V����</summary>
    [SerializeField] Animator m_anim;
 
    /// <summary>�{�[���̃I�u�W�F�N�g</summary>
    [SerializeField] Ball m_ball;
    /// <summary>�����Q�[��������̒e������</summary>
    [SerializeField] int m_ballLimit = 30;
    /// <summary>�{�[���𓊂���ʒu</summary>
    [SerializeField] GameObject m_throwPos;

    /// <summary>������Ԋu</summary>
    [SerializeField] float m_throwIntervalTime = 3;
    /// <summary>����ԍ�</summary>
    int m_ballType;

    #region Debug
    [Header("�f�o�b�N")]

    [SerializeField, Header("������Debug�Ńf�o�b�N���[�h")] DevelopType m_type;
    /// <summary>�f�o�b�N���ɓ���������������</summary>
    [SerializeField, Header("�f�o�b�N���ɓ���������������")] BallType m_debugBallType;

    #endregion

    private void Start()
    {
        GameFlowManager.Instance.OnThrowBall += () => ThrowBall();
        if (m_type == DevelopType.Debug)
        {
            m_ballLimit = 9999;
            PitcherUI.Instance.m_currentBallNum.text = "�c��̋��� : " + m_ballLimit.ToString();
        }
        m_ball = m_ball.GetComponent<Ball>();
    }

    /// <summary>
    /// �R�[���o�b�N�ŌĂ΂��֐�
    /// </summary>
    public void ThrowBall()
    {
        if (m_ballLimit == 0)
        {
            return;
        }

        if (m_type == DevelopType.Debug)
        {
            Debug.Log((int)m_debugBallType);
            m_ballType = (int)m_debugBallType;
        }
        else
        {
            m_ballType = Random.Range(0, 13);
        }

        // �A�j���[�V�����̂����ňʒu������܂���̂ŕ␳
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.position = new Vector3(0, 0, 0);

        m_ball.ChangeBallType(m_ballType);
        m_anim.SetTrigger("Throw");
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŌĂ�
    /// </summary>
    public void Throw()
    {
        m_ballLimit--;
        PitcherUI.Instance.m_currentBallNum.text = "�c��̋��� : " + m_ballLimit.ToString();
        m_ball.gameObject.SetActive(true);
    }
}

/// <summary>
/// �J���^�C�v
/// </summary>
public enum DevelopType
{
    Debug,
    Main
}