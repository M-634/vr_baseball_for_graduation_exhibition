using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ストライクゾーンを決めるクラス
/// </summary>
public class StrikeZoneChecker : MonoBehaviour
{
    /// <summary>HMDのカメラ</summary>
    [SerializeField] GameObject m_playerCamera;

    /// <summary>目線の高さ</summary>
    float m_eyeHeight;

    /// <summary>Playerの身長</summary>
    private float playerHeight;

    /// <summary>Playerの身長</summary>
    public float PlayerHeight
    {
        get { return playerHeight; }
        set { playerHeight = value; }
    }

    private void Start()
    {
        HeightMeasurement();
    }

    /// <summary>
    /// HMDのカメラを基準におおよその身長を決める関数
    /// </summary>
    public void HeightMeasurement()
    {
        m_eyeHeight = m_playerCamera.transform.position.y;
        // 大体定規で図ったら目線の高さから8～8.5cmぐらいだった
        playerHeight = m_eyeHeight + 0.085f;
        Debug.Log(playerHeight);
    }
}
