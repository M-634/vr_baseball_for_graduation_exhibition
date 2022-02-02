using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oculus��Edtior��ł̃f�o�b�O���Ǘ�����N���X.
/// </summary>
public class OculusDebugManager : SingletonMonoBehaviour<OculusDebugManager>
{
    /// <summary>OculusLink��Ńe�X�g���鎞�̃t���O</summary>
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

