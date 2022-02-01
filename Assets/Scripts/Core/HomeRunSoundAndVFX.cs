using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �z�[���������̃G�t�F�N�g�ƃT�E���h���o�𐧌䂷��N���X.
/// </summary>
public class HomeRunSoundAndVFX : MonoBehaviour
{
    /// <summary>
    /// ���[�g�I�u�W�F�N�g</summary>
    [SerializeField] GameObject m_rootObject;
    /// <summary>�ԉ΂�SFX</summary>
    [SerializeField] SFXPlayer m_fireFlowerSFX;
    /// <summary>������SFX</summary>
    [SerializeField] SFXPlayer m_cheerSFX;

    private void Awake()
    {
        if (m_rootObject == null) m_rootObject = this.gameObject;

        m_rootObject.SetActive(false);
    }

   

    /// <summary>
    /// �z�[���������o�����s����֐�.
    /// </summary>
    public void Play()
    {
        //�G�t�F�N�g���o��.
        m_rootObject.SetActive(true);

        //����炷.
        m_fireFlowerSFX.PlaySe();
        m_cheerSFX.PlaySe();
    }

    /// <summary>
    /// �z�[���������o���~�߂�֐�
    /// </summary>
    public void Stop()
    {
        m_rootObject.SetActive(false);
    }
}
