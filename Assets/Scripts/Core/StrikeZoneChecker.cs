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
    float playerHeight;

    public float PlayerHeight
    {
        get { return playerHeight; }
        set { playerHeight = value; }
    }

    /// HMD�̃J��������ɂ����悻�̐g�������߂�
    /// 

    public void HeightMeasurement()
    {

    }
}
