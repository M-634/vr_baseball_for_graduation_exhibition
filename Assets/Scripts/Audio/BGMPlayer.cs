using System;
using UnityEngine;
/// <summary>
/// BGMプレイヤー
/// </summary>
public class BGMPlayer : BasePlayer
{
    protected override void Start()
    {
        base.Start();
        m_audioSource.loop = true;
        m_audioSource.outputAudioMixerGroup = AudioManager.Instance.GetBGMAudioMixerGroup;
    }

    public void PlayBGM(AudioClip audioClip)
    {
        //再生中のBGMがあるなら止める
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }

        //BGMを再生する
        m_audioSource.pitch = 1f;
        m_audioSource.Play(audioClip);
    }


    public void StopBGM()
    {
        //再生中のBGMがあるなら止める
        if (m_audioSource.isPlaying)
        {
            m_audioSource.Stop();
        }
    }
}

