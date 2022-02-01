using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームで使用するサウンドクリップアセットの管理、
/// ゲーム全体のボリューム調整を制御するクラス.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Clip ScriptalObject")]
    [SerializeField] SoundSFXClipData m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipData m_soundBGMClipData = default;


    [Header("音源が固定されたプレイヤー")]

    /// <summary>ゲーム進行に関わる効果音や、UGUIの効果音など音源が固定されたもの</summary>
    [SerializeField] SFXPlayer m_SFXPlayer;
    /// <summary>BGMプレイヤー</summary>
    [SerializeField] BGMPlayer m_BGMPlayer;


    public void PlaySFX(KindOfSFX kindOfSFX,UnityAction callBack = null)
    {
        if(m_SFXPlayer && TryGetClip(kindOfSFX,out AudioClip audioClip))
        {
            m_SFXPlayer.PlaySeCallBack(audioClip,callBack).Forget();
        }
    }

    /// <summary>
    /// BGMを再生する関数.
    /// </summary>
    /// <param name="kindOfBGM"></param>
    public void PlayBGM(KindOfBGM kindOfBGM)
    {
        if (m_BGMPlayer && TryGetClip(kindOfBGM,out AudioClip audioClip))
        {
            m_BGMPlayer.PlayBGM(audioClip);
        }
    }

    /// <summary>
    /// SFXのAudioClipを取得する関数.
    /// </summary>
    /// <param name="kindOfSFX"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public bool TryGetClip(KindOfSFX kindOfSFX, out AudioClip audioClip)
    {
        if (!m_soundSFXClipData)
        {
            Debug.LogWarning("SFXClipデータがありません");
            audioClip = null;
            return false;
        }

        var getClip = m_soundSFXClipData.GetClip(kindOfSFX);

        if (!getClip)
        {
            Debug.LogWarning($"{kindOfSFX}のclipがありません");
            audioClip = null;
            return false;
        }

        audioClip = getClip;
        return true;
    }

    /// <summary>
    ///  BGMのAudioClipを取得する関数.
    /// </summary>
    /// <param name="kindOfBGM"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    private bool TryGetClip(KindOfBGM kindOfBGM, out AudioClip audioClip)
    {
        if (!m_soundBGMClipData)
        {
            Debug.LogWarning("BGMClipデータがありません");
            audioClip = null;
            return false;
        }

        var getClip = m_soundBGMClipData.GetClip(kindOfBGM);

        if (!getClip)
        {
            Debug.LogWarning($"{kindOfBGM}のclipがありません");
            audioClip = null;
            return false;
        }

        audioClip = getClip;
        return true;
    }

    public void StopBGM()
    {
        if (m_BGMPlayer) m_BGMPlayer.StopBGM();
    }
}
