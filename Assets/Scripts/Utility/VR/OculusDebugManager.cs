using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OculusとEdtior上でのデバッグを管理するクラス.
/// </summary>
public class OculusDebugManager : SingletonMonoBehaviour<OculusDebugManager>
{
    /// <summary>OculusLink上でテストする時のフラグ</summary>
    public bool m_debugOculusLink = false;
 
    public bool DebugOculusLink => m_debugOculusLink;

    [SerializeField] GameObject debugCamera;
    [SerializeField] GameObject XRRig;

    protected override void Awake()
    {
        base.Awake();

        debugCamera.SetActive(!m_debugOculusLink);
        XRRig.SetActive(m_debugOculusLink);
    }
}

