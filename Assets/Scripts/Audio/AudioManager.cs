using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �Q�[���Ŏg�p����T�E���h�N���b�v�A�Z�b�g�̊Ǘ��A
/// �Q�[���S�̂̃{�����[�������𐧌䂷��N���X.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [Header("Audio Clip ScriptalObject")]
    [SerializeField] SoundSFXClipData m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipData m_soundBGMClipData = default;


    [Header("�������Œ肳�ꂽ�v���C���[")]

    /// <summary>�Q�[���i�s�Ɋւ����ʉ���AUGUI�̌��ʉ��Ȃǉ������Œ肳�ꂽ����</summary>
    [SerializeField] SFXPlayer m_SFXPlayer;
    /// <summary>BGM�v���C���[</summary>
    [SerializeField] BGMPlayer m_BGMPlayer;


    public void PlaySFX(KindOfSFX kindOfSFX,UnityAction callBack = null)
    {
        if(m_SFXPlayer && TryGetClip(kindOfSFX,out AudioClip audioClip))
        {
            m_SFXPlayer.PlaySeCallBack(audioClip,callBack).Forget();
        }
    }

    /// <summary>
    /// BGM���Đ�����֐�.
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
