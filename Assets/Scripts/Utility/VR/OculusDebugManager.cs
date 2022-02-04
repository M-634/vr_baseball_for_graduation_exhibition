using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OculusLink,���@�iOculus Quest2�j��Edtior��ł̃e�X�g���̐�����Ǘ�����N���X.
/// </summary>
public class OculusDebugManager : SingletonMonoBehaviour<OculusDebugManager>
{
    /// <summary>
    /// Editor��ł̃e�X�g�ł�true.OculusLink�܂��́A���@Build����false�ɂ���.
    /// OculusLink���g�p�����Ƀe�X�g���������́A�C���X�y�N�^�[��Ń`�F�b�N�����Ă�������
    /// </summary>
    [SerializeField] bool m_debugEditorMode = false;
    
    public  bool DebugEditorMode => m_debugEditorMode;

    /// <summary>
    /// Editor��ł̃e�X�g���鎞�̃I�u�W�F�N�g���܂Ƃ߂����[�g�I�u�W�F�N�g.
    /// �q�G�����L�[���Unity3D�̃f�t�H���g�J����(DebugCamera���Ė��������Ă���Q�[���I�u�W�F�N�g)�ɊY������.
    /// </summary>
    [SerializeField] GameObject debugCamera;

    /// <summary>
    /// XR Control Tool kit��XR Rig���A�T�C������.
    /// </summary>
    [SerializeField] GameObject XRRig;

    protected override void Awake()
    {
        base.Awake();

        debugCamera.SetActive(m_debugEditorMode);
        XRRig.SetActive(!m_debugEditorMode);
    }
}

