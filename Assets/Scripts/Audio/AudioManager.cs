using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// ゲームで使用するサウンドクリップアセットの管理、
/// ゲーム全体のボリューム調整を制御するクラス.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Clip ScriptalObject")]
    [SerializeField] SoundSFXClipDataSO m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipDataSO m_soundBGMClipData = default;


    [Header("Audio Mixer")]
    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] AudioMixerGroup m_bgmAMG, m_sfxAMG;


    [Header("音源が固定されたプレイヤー")]
    /// <summary>ゲーム進行に関わる効果音や、UGUIの効果音など音源が固定されたもの</summary>
    [SerializeField] SFXPlayer m_SFXPlayer;
    /// <summary>BGMプレイヤー</summary>
    [SerializeField] BGMPlayer m_BGMPlayer;

    [Header("音量設定")]
    [SerializeField] SoundVolumeSO m_soundVolumeSO = default;
    [SerializeField] Slider m_masterSlider, m_bgmSlider, m_sfxSlider;


    public AudioMixerGroup GetBGMAudioMixerGroup => m_bgmAMG;
    public AudioMixerGroup GetSFXAudioMixerGroup => m_sfxAMG;

    private const string MasterVolumeParaName = "MasterVolume";
    private const string BGMVolumeParaName = "BGMVolume";
    private const string SFXVolumeParaName = "SFXVolume";

    public float MasterVolume
    {
        get => m_audioMixer.GetVolumeByLinear(MasterVolumeParaName);
        set 
        {
            m_audioMixer.SetVolumeByLinear(MasterVolumeParaName, value);
            m_soundVolumeSO.MasterVolume = value;
        }
    }

    public float BGMVolume
    {
        get => m_audioMixer.GetVolumeByLinear(BGMVolumeParaName);
        set
        {
            m_audioMixer.SetVolumeByLinear(BGMVolumeParaName, value);
            m_soundVolumeSO.BGMVolume = value;
        }
    }

    public float SFXVolume
    {
        get => m_audioMixer.GetVolumeByLinear(SFXVolumeParaName);
        set
        {
            m_audioMixer.SetVolumeByLinear(SFXVolumeParaName, value);
            m_soundVolumeSO.SFXVolume = value;
        }
    }


    private void Start()
    {
        //VR上でスライダーを動かした時のみ
        SetAudioVolumesEvent(m_masterSlider, vol =>MasterVolume = vol);
        SetAudioVolumesEvent(m_bgmSlider, vol => BGMVolume = vol);
        SetAudioVolumesEvent(m_sfxSlider, vol => SFXVolume = vol);

        //スライダーを初期化
        m_masterSlider.SetSliderValue(m_soundVolumeSO.MasterVolume);
        m_bgmSlider.SetSliderValue(m_soundVolumeSO.BGMVolume);
        m_sfxSlider.SetSliderValue(m_soundVolumeSO.SFXVolume);
    }


#if UNITY_EDITOR
    private void Update()
    {
        //Unity Editor上で音量を調整する
        MasterVolume = m_soundVolumeSO.MasterVolume;
        BGMVolume = m_soundVolumeSO.BGMVolume;
        SFXVolume = m_soundVolumeSO.SFXVolume;
    }
#endif


    private void SetAudioVolumesEvent(Slider slider, UnityAction<float> callBack)
    {
        slider.SetValueChangedEvent(callBack);
    }


    public void PlaySFX(KindOfSFX kindOfSFX, UnityAction callBack = null)
    {
        if (m_SFXPlayer && TryGetClip(kindOfSFX, out AudioClip audioClip))
        {
            m_SFXPlayer.PlaySeCallBack(audioClip, callBack).Forget();
        }
    }

    /// <summary>
    /// BGMを再生する関数.
    /// </summary>
    /// <param name="kindOfBGM"></param>
    public void PlayBGM(KindOfBGM kindOfBGM)
    {
        if (m_BGMPlayer && TryGetClip(kindOfBGM, out AudioClip audioClip))
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
