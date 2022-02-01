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
    /// <summary>このコンポーネントをアタッチしているオブジェクトが出す効果音を指定する</summary>
    [SerializeField] KindOfSFX m_kindOfSFX;

   [SerializeField] AudioSource m_audioSource;
   [SerializeField] ONSPAudioSource m_ONSPAudioSource;

    /// <summary>
    /// このコンポーネントをアタッチされた時に呼ばれる関数
    /// </summary>
    private void Reset()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_ONSPAudioSource = GetComponent<ONSPAudioSource>();
    }


    private void Start()
    {
        //AudioSourceの設定
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 0f;

        //ONSPAudioSourceの設定
        m_ONSPAudioSource.EnableSpatialization = true;
    }


    /// <summary>
    /// SFXを再生する.
    /// </summary>
    public void PlaySe()
    {
        if (AudioManager.Instance.TryGetClip(m_kindOfSFX, out AudioClip audioClip))
        {
            m_audioSource.pitch = 1f;
            m_audioSource.Play(audioClip);
        }

    }

    /// <summary>
    /// pitchをランダムにして音を鳴らす（繰り返し音を鳴らすものなどに）
    /// </summary>
    public void PlayPitchRandomize(float range = 0.5f)
    {

        if (AudioManager.Instance.TryGetClip(m_kindOfSFX, out AudioClip audioClip))
        {
            m_audioSource.pitch = Random.Range(1f - range, 1f + range);
            m_audioSource.Play(audioClip);
        }
    }


    /// <summary>
    /// 引数にpitchを与えて音を鳴らす（バッティングの音に使用する）
    /// </summary>
    public void PlaySetPitch(float pitch = 1f)
    {

        if (AudioManager.Instance.TryGetClip(m_kindOfSFX, out AudioClip audioClip))
        {
            m_audioSource.pitch = Mathf.Clamp(pitch, -3f, 3f);
            m_audioSource.Play(audioClip);
        }
    }

}

