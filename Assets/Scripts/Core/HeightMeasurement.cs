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
    float playerHeight;

    public float PlayerHeight
    {
        get { return playerHeight; }
        set { playerHeight = value; }
    }

    /// HMDのカメラを基準におおよその身長を決める
    /// 

    public void HeightMeasurement()
    {

    }
}
