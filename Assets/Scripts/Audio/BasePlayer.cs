using UnityEngine;

/// <summary>
/// Oculus用、3D空間上での効果音プレイヤーのベースクラス.
/// Oculus Audio SDKをインポートしてください.
/// </summary>
[RequireComponent(typeof(AudioSource), (typeof(ONSPAudioSource)))]
public abstract class BasePlayer : MonoBehaviour 
{
    [SerializeField] protected AudioSource m_audioSource;
    [SerializeField] protected ONSPAudioSource m_ONSPAudioSource;

    /// <summary>
    /// このコンポーネントをアタッチされた時に呼ばれる関数
    /// </summary>
    private void Reset()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_ONSPAudioSource = GetComponent<ONSPAudioSource>();
    }

    protected virtual void Start()
    {
        //AudioSourceの設定
        m_audioSource.playOnAwake = false;
        m_audioSource.spatialBlend = 0f;

        //ONSPAudioSourceの設定
        m_ONSPAudioSource.EnableSpatialization = true;
    }
}

