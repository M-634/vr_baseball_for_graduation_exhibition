using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ŋ��̃q�b�g������h�~����N���X.
/// �n�ʃI�u�W�F�b�g�ɃA�b�^�b�`���Ă�������.
/// </summary>
public class Ground : MonoBehaviour, IBallHitObjet
{
    [SerializeField] bool doHitCheack = true;
    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        if (!doHitCheack) return;

        //�ʒu�␳
        var tempPos = rb.transform.position;
        tempPos.y = hitObjectInfo.point.y + 0.05f;
        rb.transform.position = tempPos;

        //���x�ϊ�
        //world���Wy��������̒e���Փ�
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * -1f * BaseBallLogic.CoefficientOfRestitution, rb.velocity.z);
    }
}
