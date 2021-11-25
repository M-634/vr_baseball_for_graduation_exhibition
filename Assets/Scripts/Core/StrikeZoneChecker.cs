using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�g���C�N�]�[�����m�F����N���X
/// </summary>
public class StrikeZoneChecker : MonoBehaviour
{
    /// <summary>HMD�̃J����</summary>
    [SerializeField] GameObject m_playerCamera;
    /// <summary>�X�g���C�N���ʗp�I�u�W�F�N�g</summary>
    [SerializeField] GameObject m_strikeZoneObj;

    /// <summary>�ڐ��̍���</summary>
    float m_eyeHeight;

    /// <summary>Player�̐g��</summary>
    private float playerHeight;

    /// <summary>���g</summary>
    readonly int headsHigh = 8;

    /// <summary>�X�g���C�N�]�[���̈�ԍ����Ƃ�</summary>
    float m_strikeZoneHeightMax;
    /// <summary>�X�g���C�N�]�[���̈�ԒႢ�Ƃ�</summary>
    float m_strikeZoneHeightMin;

    private void Start()
    {
        StartCoroutine(HeightMeasurement());
    }

    /// <summary>
    /// HMD�̃J��������ɂ����悻�̐g�������߂�֐�
    /// </summary>
    public IEnumerator HeightMeasurement()
    {
        yield return new WaitForSeconds(2);

        m_eyeHeight = m_playerCamera.transform.position.y;
        
        playerHeight = m_eyeHeight + 0.175f;
        Debug.Log(playerHeight);

        StrikeZoneDecide();
    }

    /// <summary>
    /// �X�g���C�N�]�[�������߂�֐�
    /// </summary>
    public void StrikeZoneDecide()
    {
        // 1���g������̑傫��
        float oneHeadsHigh = playerHeight / headsHigh;
        // ���̂܂ł̍���
        m_strikeZoneHeightMax = playerHeight - (oneHeadsHigh * 2f);
        // �G���܂ł̍���
        m_strikeZoneHeightMin = playerHeight - (oneHeadsHigh * 6f);

        m_strikeZoneObj.SetActive(true);

        m_strikeZoneObj.transform.localScale = new Vector3(m_strikeZoneObj.transform.localScale.x, m_strikeZoneHeightMax - m_strikeZoneHeightMin, m_strikeZoneObj.transform.localScale.z);
        m_strikeZoneObj.transform.position = new Vector3(m_strikeZoneObj.transform.position.x, (m_strikeZoneHeightMax - m_strikeZoneHeightMin) / 2 + m_strikeZoneHeightMin, m_strikeZoneObj.transform.position.z);
    }
}
