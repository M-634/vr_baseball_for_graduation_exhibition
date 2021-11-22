using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �o�b�g������ł��Ԃ����ɔ�������͊w�I�ȉ^���𐧌䂷��N���X.
/// </summary>
public class BatControl : MonoBehaviour, IBallHitObjet
{
    /// <summary>�o�b�g�̒��S�ʒu</summary>
    [SerializeField] Transform barycenter;
    /// <summary>�o�b�g�̃O���b�v�ʒu</summary>
    [SerializeField] Transform grip;

    [SerializeField] float debugRotatePower = -10;
    /// <summary>�o�b�g�ɓ��������u�ԂɃt���O�𗧂Ă�</summary>
    private bool onImpact = false;
    /// <summary>1�t���[���O�̒��S�ʒu</summary>
    private Vector3 lastBarycenterPos;
    /// <summary>�o�b�g���S�̑��x</summary>
    private float barycenterVelocity;
    /// <summary>�C���p�N�g���̃o�b�g���S�̃x�N�g���̌���</summary>
    private Vector3 barycenterVectorOnImpact;
    /// <summary>�O���b�v����o�b�g�̒��S�܂ł̒����i�~�^���ɂ����锼�a�j</summary>
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        lastBarycenterPos = barycenter.position;
    }


    private void FixedUpdate()
    {

        transform.RotateAround(grip.position,Vector3.up, debugRotatePower);

        barycenterVelocity = GetBarycenterOfBatVelocity(lastBarycenterPos, barycenter.position);

        barycenterVectorOnImpact = (barycenter.position - lastBarycenterPos).normalized;

        //Debug.Log(barycenterVelocity * 1000);
        //Debug.DrawRay(barycenter.position, barycenterVectorOnImpact, Color.white);

        lastBarycenterPos = barycenter.position;
    }

    /// <summary>
    /// �o�b�g�̏d�S���O���b�h�𒆐S�Ƃ���~�^�����瓾���鑬�x�̑傫����Ԃ��֐�.
    /// </summary>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>
    /// <returns></returns>
    private float GetBarycenterOfBatVelocity(Vector3 lastPos,Vector3 currentPos)
    {
        var lhs = (lastPos - grip.position).normalized;
        var rhs = (currentPos - grip.position).normalized;

        var dot = Vector3.Dot(lhs, rhs);

        if (dot == 1) return 0f;

        //1�t���[���O��̃x�N�g������p�x�����߂�
        float angle = Mathf.Acos(dot);// cos�� = dot / |a| * |b| :   |a| = |b| = 1
        //�p�x�����W�A���ɕϊ�
        float radian = angle * Mathf.PI / 180;
        //�p���x�����߂�
        float barycenterAngularVelocity = radian / Time.fixedDeltaTime; //�� = ���� / ��t
        //�O���b�v����o�b�g�̒��S�܂ł̋����𑪂�
        radius = (barycenter.position - grip.position).magnitude;
        //�o�b�g���S�̑��x�����߂� ; v = r��
        return radius * barycenterAngularVelocity;
    }

    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        rb.velocity = hitObjectInfo.normal * BattingPower(hitObjectInfo, ballSpeed);
        Debug.Log("finalpower :" + BattingPower(hitObjectInfo, ballSpeed));
    }

    private float BattingPower(RaycastHit hitObjectInfo ,float ballSpeed)
    {
        //�C���p�N�g�̏u�Ԃ̃o�b�g�̌����ƃ{�[���̌����̓��ό��ʂɂ��␳�l��������
        float dir = Vector3.Dot(barycenterVectorOnImpact, hitObjectInfo.normal);
        dir = Mathf.Clamp01(dir);

        //�{�[�������������ꏊ����o�b�g�̏d�S�܂ł̋���������,�����ɉ����ăp���[�ɕ␳�l��������
        float length = Vector3.Distance(hitObjectInfo.point, barycenter.position) * 100;
        //�o�b�g�̐c�ɋ߂���1�ɋ߂��l��Ԃ��A�o�b�g�̐c����̋�����17cm�������0��Ԃ�
        length = Mathf.Clamp01(1 - length / 17);

        Debug.Log("���� :" + ballSpeed * dir);
        Debug.Log("���[�����g: " + barycenterVelocity * length);
        //�ŏI�I�ȃ{�[���ɉ�����͂��v�Z���ʂ�Ԃ��i�����C������)
        return  ballSpeed * dir + barycenterVelocity * length;
    }

}
