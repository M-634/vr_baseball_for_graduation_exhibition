using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �s�b�`���[�����UI�Ǘ�
/// </summary>
public class PitcherUI : MonoBehaviour
{
    /// <summary�{�[���̍���������Slider</summary>
    [SerializeField] public Slider m_heightAdjust;

    private static PitcherUI m_instance;
    public static PitcherUI Instance
    {
        get
        {
            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance != null && m_instance == this)
        {
            Destroy(gameObject);
        }

        m_instance = this;
    }

    private void Start()
    {
        m_heightAdjust.value = 0f;
    }

    /// <summary>
    /// �{�^�����������ƍ����𒲐�����֐�
    /// </summary>
    /// <param name="adjustNum"></param>
    public void AdjustHeight(float adjustNum)
    {
        m_heightAdjust.value += adjustNum;
    }
}
