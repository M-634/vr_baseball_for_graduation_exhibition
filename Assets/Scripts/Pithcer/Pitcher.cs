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

    [SerializeField, Header("������Debug�ŉi���X�g���[�g�̌Y")] DevelopType m_type;

    private void Start()
    {
        StartCoroutine(ThrowInterval());
        m_ball = m_ball.GetComponent<Ball>();
    }

    IEnumerator ThrowInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_throwIntervalTime);
            if (m_type == DevelopType.Debug)
            {
                m_ballType = 0;
            }
            else
            {
                m_ballType = Random.Range(0, 9);
            }
            
            m_ball.ChangeBallType(m_ballType);
            m_anim.SetTrigger("Throw");
        }
    }

    /// <summary>
    /// �A�j���[�V�����C�x���g�ŌĂ�
    /// </summary>
    public void Throw()
    {
        if (m_ballLimit != 0)
        {
            m_ballLimit -= 1;
            PitcherUI.Instance.m_currentBallNum.text = "�c��̋��� : " + m_ballLimit.ToString();
            m_ball.transform.position = m_throwPos.transform.position;
            m_ball.gameObject.SetActive(true);
        }
    }
}

public enum DevelopType
{
    Debug,
    Main
}