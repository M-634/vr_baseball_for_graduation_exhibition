using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ホームラン時のエフェクトとサウンド演出を制御するクラス.
/// </summary>
public class HomeRunSoundAndVFX : MonoBehaviour
{
    /// <summary>
    /// ルートオブジェクト</summary>
    [SerializeField] GameObject m_rootObject;
    /// <summary>花火のSFX</summary>
    [SerializeField] SFXPlayer m_fireFlowerSFX;
    /// <summary>歓声のSFX</summary>
    [SerializeField] SFXPlayer m_cheerSFX;

    private void Awake()
    {
        if (m_rootObject == null) m_rootObject = this.gameObject;

        m_rootObject.SetActive(false);
    }

   

    /// <summary>
    /// ホームラン演出を実行する関数.
    /// </summary>
    public void Play()
    {
        //エフェクトを出す.
        m_rootObject.SetActive(true);

        //音を鳴らす.
        m_fireFlowerSFX.PlaySe();
        m_cheerSFX.PlaySe();
    }

    /// <summary>
    /// ホームラン演出を止める関数
    /// </summary>
    public void Stop()
    {
        m_rootObject.SetActive(false);
    }
}
