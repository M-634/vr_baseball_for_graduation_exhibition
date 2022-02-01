using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// 効果音プレイヤー.
/// </summary>
public class SFXPlayer : BasePlayer
{
    /// <summary>このコンポーネントをアタッチしているオブジェクトが出す効果音を指定する</summary>
    [SerializeField] KindOfSFX m_kindOfSFX;

    protected override void Start()
    {
        base.Start();
        m_audioSource.loop = false;
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
    /// SFXを再生させ、終わったらコールバックを呼ぶ関数.
    /// </summary>
    public async UniTaskVoid PlaySeCallBack(AudioClip audioClip, UnityAction callback = null)
    {
        m_audioSource.pitch = 1f;
        m_audioSource.Play(audioClip);

        //再生が終わるまで待機
        await UniTask.WaitUntil(() => m_audioSource.isPlaying == false);

        //コールバック
        callback?.Invoke();
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

