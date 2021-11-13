using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ł����ŋ��̔�����s���N���X.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class HitCheck : MonoBehaviour,IBallHitObjet
{
    /// <summary>���̃q�b�g������s���I�u�W�F�N�g�Ȃ̂��\�ߌ��߂Ă���</summary>
    [SerializeField] HitType hitType;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnHit(Rigidbody rb, Vector3 normal, float ballSpeed)
    {
        switch (hitType)
        {
            case HitType.Hit:
                Debug.Log("hit");
                break;
            case HitType.TwoBase:
                break;
            case HitType.ThreeBase:
                break;
            case HitType.HomeRun:
                break;
            case HitType.Foul:
                break;
            case HitType.Out:
                break;
            default:
                break;
        }
    }
}

public enum HitType
{
    Hit,TwoBase,ThreeBase,HomeRun,Foul,Out
}
