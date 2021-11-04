using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// ピッチャー周りのUI管理
/// </summary>
public class PitcherUI : MonoBehaviour
{
    /// <summaryボールの高さ調整のSlider</summary>
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
        m_heightAdjust.value = 0.5f;
    }

    /// <summary>
    /// ボタンが押されると高さを調整する関数
    /// </summary>
    /// <param name="adjustNum"></param>
    public void AdjustHeight(float adjustNum)
    {
        if (m_heightAdjust.value == 0 || m_heightAdjust.value == 1)
        {
            return;
        }
        m_heightAdjust.value -= adjustNum;
    }
}
