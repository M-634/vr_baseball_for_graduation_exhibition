using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Oculus�p�A3D��ԏ�ł̌��ʉ��v���C���[.
/// Oculus Audio SDK���C���|�[�g���Ă�������.
/// </summary>
[RequireComponent(typeof(AudioSource), (typeof(ONSPAudioSource)))]
public class SFXPlayer : MonoBehaviour
{
    /// <summary>���̃R���|�[�l���g���A�^�b�`���Ă���I�u�W�F�N�g���o�����ʉ����w�肷��</summary>
    [SerializeField] KindOfSFX m_kindOfSFX;

   [SerializeField] AudioSource m_audioSource;
   [SerializeField] ONSPAudioSource m_ONSPAudioSource;

    /// <summary>
    /// ���̃R���|�[�l���g���A�^�b�`���ꂽ���ɌĂ΂��֐�
    /// </summary>
    private void Reset()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_ONSPAudioSource = GetComponent<ONSPAudioSource>();
    }


    private void Start()
    {
        //AudioSource�̐ݒ�
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 0f;

        //ONSPAudioSource�̐ݒ�
        m_ONSPAudioSource.EnableSpatialization = true;
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

