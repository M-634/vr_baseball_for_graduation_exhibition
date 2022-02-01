using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームで使用するサウンドクリップアセットの管理、
/// ゲーム全体のボリューム調整を制御するクラス.
/// </summary>
public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField] SoundSFXClipData m_soundSFXClipData = default;
    [SerializeField] SoundBGMClipData m_soundBGMClipData = default;

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

    public bool TryGetClip(KindOfBGM kindOfBGM, out AudioClip audioClip)
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

}
