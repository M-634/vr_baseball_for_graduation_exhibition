using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ストライクゾーンを確認するクラス
/// </summary>
public class StrikeZoneChecker : MonoBehaviour
{
    /// <summary>HMDのカメラ</summary>
    [SerializeField] GameObject m_playerCamera;
    /// <summary>ストライク判別用オブジェクト</summary>
    [SerializeField] GameObject m_strikeZoneObj;

    /// <summary>目線の高さ</summary>
    float m_eyeHeight;

    /// <summary>Playerの身長</summary>
    private float playerHeight;

    /// <summary>頭身</summary>
    readonly int headsHigh = 8;

    /// <summary>ストライクゾーンの一番高いとこ</summary>
    float m_strikeZoneHeightMax;
    /// <summary>ストライクゾーンの一番低いとこ</summary>
    float m_strikeZoneHeightMin;

    private void Start()
    {
        StartCoroutine(HeightMeasurement());
    }

    /// <summary>
    /// HMDのカメラを基準におおよその身長を決める関数
    /// </summary>
    public IEnumerator HeightMeasurement()
    {
        yield return new WaitForSeconds(2);

        m_eyeHeight = m_playerCamera.transform.position.y;
        
        playerHeight = m_eyeHeight + 0.175f;
        Debug.Log(playerHeight);

        StrikeZoneDecide();
    }

    /// <summary>
    /// ストライクゾーンを決める関数
    /// </summary>
    public void StrikeZoneDecide()
    {
        // 1頭身あたりの大きさ
        float oneHeadsHigh = playerHeight / headsHigh;
        // 胸のまでの高さ
        m_strikeZoneHeightMax = playerHeight - (oneHeadsHigh * 2f);
        // 膝下までの高さ
        m_strikeZoneHeightMin = playerHeight - (oneHeadsHigh * 6f);

        m_strikeZoneObj.SetActive(true);

        m_strikeZoneObj.transform.localScale = new Vector3(m_strikeZoneObj.transform.localScale.x, m_strikeZoneHeightMax - m_strikeZoneHeightMin, m_strikeZoneObj.transform.localScale.z);
        m_strikeZoneObj.transform.position = new Vector3(m_strikeZoneObj.transform.position.x, (m_strikeZoneHeightMax - m_strikeZoneHeightMin) / 2 + m_strikeZoneHeightMin, m_strikeZoneObj.transform.position.z);
    }
}
