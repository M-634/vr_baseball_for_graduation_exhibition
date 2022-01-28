using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oculus用、3D空間上での効果音プレイヤー.
/// Oculus Audio SDKをインポートしてください.
/// </summary>
[RequireComponent(typeof(AudioSource), (typeof(ONSPAudioSource)))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> m_audioClipList = new List<AudioClip>();
    private AudioSource m_audioSource;
    private ONSPAudioSource m_ONSPAudioSource;

    /// <summary>
    /// このコンポーネントをアタッチされた時に呼ばれる関数
    /// </summary>
    private void Start()
    {
        //AudioSourceの設定
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 0f;

        //ONSPAudioSourceの設定
        m_ONSPAudioSource = GetComponent<ONSPAudioSource>();
        m_ONSPAudioSource.EnableSpatialization = true;
    }

    public void PlayFirstAudioClip()
    {
        if (m_audioClipList.Count > 0)
        {
            m_audioSource.pitch = 1f;
            m_audioSource.Play(m_audioClipList[0]);
        }
    }

    /// <summary>
    /// 引数に与えられたピッチに応じて効果音を再生する関数.
    /// </summary>
    public void PlayFirstAudioClipSetPitch(float pitch = 1f)
    {
        if (m_audioClipList.Count > 0)
        {
            m_audioSource.pitch = Mathf.Clamp(pitch, -3f, 3f);
            m_audioSource.Play(m_audioClipList[0]);
            Debug.Log("aaa");
        }
    }

    public void PlaySe(string clipName)
    {
        var audioClip = m_audioClipList.FirstOrDefault(clip => clip.name == clipName);

        if (audioClip)
        {
            m_audioSource.pitch = 1f;
            m_audioSource.Play(audioClip);
        }
    }

    /// <summary>
    /// pitchをランダムにして音を鳴らす（繰り返し音を鳴らすものなどに）
    /// </summary>
    public void PlaySePitchRandomize(string clipName, float range = 0.5f)
    {
        var audioClip = m_audioClipList.FirstOrDefault(clip => clip.name == clipName);

        if (audioClip)
        {
            m_audioSource.pitch = Random.Range(1f - range, 1f + range);
            m_audioSource.Play(audioClip);
        }
    }

}

