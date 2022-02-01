using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

/// <summary>
/// ���ʉ��v���C���[.
/// </summary>
public class SFXPlayer : BasePlayer
{
    /// <summary>���̃R���|�[�l���g���A�^�b�`���Ă���I�u�W�F�N�g���o�����ʉ����w�肷��</summary>
    [SerializeField] KindOfSFX m_kindOfSFX;

    protected override void Start()
    {
        base.Start();
        m_audioSource.loop = false;
    }

    /// <summary>
    /// SFX���Đ�����.
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
    /// SFX���Đ������A�I�������R�[���o�b�N���ĂԊ֐�.
    /// </summary>
    public async UniTaskVoid PlaySeCallBack(AudioClip audioClip, UnityAction callback = null)
    {
        m_audioSource.pitch = 1f;
        m_audioSource.Play(audioClip);

        //�Đ����I���܂őҋ@
        await UniTask.WaitUntil(() => m_audioSource.isPlaying == false);

        //�R�[���o�b�N
        callback?.Invoke();
    }

    /// <summary>
    /// pitch�������_���ɂ��ĉ���炷�i�J��Ԃ�����炷���̂ȂǂɁj
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
    /// ������pitch��^���ĉ���炷�i�o�b�e�B���O�̉��Ɏg�p����j
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

