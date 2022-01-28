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
    [SerializeField] List<AudioClip> m_audioClipList = new List<AudioClip>();
    private AudioSource m_audioSource;
    private ONSPAudioSource m_ONSPAudioSource;

    /// <summary>
    /// ���̃R���|�[�l���g���A�^�b�`���ꂽ���ɌĂ΂��֐�
    /// </summary>
    private void Start()
    {
        //AudioSource�̐ݒ�
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.playOnAwake = false;
        m_audioSource.loop = false;
        m_audioSource.spatialBlend = 0f;

        //ONSPAudioSource�̐ݒ�
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
    /// �����ɗ^����ꂽ�s�b�`�ɉ����Č��ʉ����Đ�����֐�.
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
    /// pitch�������_���ɂ��ĉ���炷�i�J��Ԃ�����炷���̂ȂǂɁj
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

