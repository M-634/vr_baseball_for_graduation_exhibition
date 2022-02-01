using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �Q�[���Ŏg�p����T�E���h�N���b�v�A�Z�b�g�̊Ǘ��A
/// �Q�[���S�̂̃{�����[�������𐧌䂷��N���X.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] SoundSFXClipData m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipData m_soundBGMClipData = default;

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

    public bool TryGetClip(KindOfBGM kindOfBGM, out AudioClip audioClip)
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

}
