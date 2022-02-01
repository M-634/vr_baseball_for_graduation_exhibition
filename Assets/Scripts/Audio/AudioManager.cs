using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// �Q�[���Ŏg�p����T�E���h�N���b�v�A�Z�b�g�̊Ǘ��A
/// �Q�[���S�̂̃{�����[�������𐧌䂷��N���X.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Clip ScriptalObject")]
    [SerializeField] SoundSFXClipDataSO m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipDataSO m_soundBGMClipData = default;


    [Header("Audio Mixer")]
    [SerializeField] AudioMixer m_audioMixer;
    [SerializeField] AudioMixerGroup m_bgmAMG, m_sfxAMG;


    [Header("�������Œ肳�ꂽ�v���C���[")]
    /// <summary>�Q�[���i�s�Ɋւ����ʉ���AUGUI�̌��ʉ��Ȃǉ������Œ肳�ꂽ����</summary>
    [SerializeField] SFXPlayer m_SFXPlayer;
    /// <summary>BGM�v���C���[</summary>
    [SerializeField] BGMPlayer m_BGMPlayer;

    [Header("���ʐݒ�")]
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
        //VR��ŃX���C�_�[�𓮂��������̂�
        SetAudioVolumesEvent(m_masterSlider, vol =>MasterVolume = vol);
        SetAudioVolumesEvent(m_bgmSlider, vol => BGMVolume = vol);
        SetAudioVolumesEvent(m_sfxSlider, vol => SFXVolume = vol);

        //�X���C�_�[��������
        m_masterSlider.SetSliderValue(m_soundVolumeSO.MasterVolume);
        m_bgmSlider.SetSliderValue(m_soundVolumeSO.BGMVolume);
        m_sfxSlider.SetSliderValue(m_soundVolumeSO.SFXVolume);
    }


#if UNITY_EDITOR
    private void Update()
    {
        //Unity Editor��ŉ��ʂ𒲐�����
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
    /// BGM���Đ�����֐�.
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
    /// SFX��AudioClip���擾����֐�.
    /// </summary>
    /// <param name="kindOfSFX"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public bool TryGetClip(KindOfSFX kindOfSFX, out AudioClip audioClip)
    {
        if (!m_soundSFXClipData)
        {
            Debug.LogWarning("SFXClip�f�[�^������܂���");
            audioClip = null;
            return false;
        }

        var getClip = m_soundSFXClipData.GetClip(kindOfSFX);

        if (!getClip)
        {
            Debug.LogWarning($"{kindOfSFX}��clip������܂���");
            audioClip = null;
            return false;
        }

        audioClip = getClip;
        return true;
    }

    /// <summary>
    ///  BGM��AudioClip���擾����֐�.
    /// </summary>
    /// <param name="kindOfBGM"></param>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    private bool TryGetClip(KindOfBGM kindOfBGM, out AudioClip audioClip)
    {
        if (!m_soundBGMClipData)
        {
            Debug.LogWarning("BGMClip�f�[�^������܂���");
            audioClip = null;
            return false;
        }

        var getClip = m_soundBGMClipData.GetClip(kindOfBGM);

        if (!getClip)
        {
            Debug.LogWarning($"{kindOfBGM}��clip������܂���");
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
