using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OculusLink,実機（Oculus Quest2）とEdtior上でのテスト時の制御を管理するクラス.
/// </summary>
public class OculusDebugManager : SingletonMonoBehaviour<OculusDebugManager>
{
    /// <summary>
    /// Editor上でのテストではtrue.OculusLinkまたは、実機Build時はfalseにする.
    /// OculusLinkを使用せずにテストしたい時は、インスペクター上でチェックを入れてください
    /// </summary>
    [SerializeField] bool m_debugEditorMode = false;
    
    public  bool DebugEditorMode => m_debugEditorMode;

    /// <summary>
    /// Editor上でのテストする時のオブジェクトをまとめたルートオブジェクト.
    /// ヒエラルキー上のUnity3Dのデフォルトカメラ(DebugCameraって命名がついているゲームオブジェクト)に該当する.
    /// </summary>
    [SerializeField] GameObject debugCamera;

    /// <summary>
    /// XR Control Tool kitのXR Rigをアサインする.
    /// </summary>
    [SerializeField] GameObject XRRig;

    protected override void Awake()
    {
        base.Awake();

        debugCamera.SetActive(m_debugEditorMode);
        XRRig.SetActive(!m_debugEditorMode);
    }
}

