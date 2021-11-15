using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �X�g���C�N�]�[�������߂�N���X
/// </summary>
public class StrikeZoneChecker : MonoBehaviour
{
    /// <summary>HMD�̃J����</summary>
    [SerializeField] GameObject m_playerCamera;

    /// <summary>�ڐ��̍���</summary>
    float m_eyeHeight;

    /// <summary>Player�̐g��</summary>
    private float playerHeight;

    /// <summary>Player�̐g��</summary>
    public float PlayerHeight
    {
        get { return playerHeight; }
        set { playerHeight = value; }
    }

    private void Start()
    {
        HeightMeasurement();
    }

    /// <summary>
    /// HMD�̃J��������ɂ����悻�̐g�������߂�֐�
    /// </summary>
    public void HeightMeasurement()
    {
        m_eyeHeight = m_playerCamera.transform.position.y;
        // ��̒�K�Ő}������ڐ��̍�������8�`8.5cm���炢������
        playerHeight = m_eyeHeight + 0.085f;
        Debug.Log(playerHeight);
    }
}
