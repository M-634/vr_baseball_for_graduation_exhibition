using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �o�b�g������ł��Ԃ����ɔ�������͊w�I�ȉ^���𐧌䂷��N���X.
/// </summary>
public class BatControl : MonoBehaviour, IBallHitObjet
{

    [Header("�eTransfom���Z�b�g����")]
    /// <summary>�o�b�g�̒��S�ʒu</summary>
    [SerializeField] Transform barycenter;
    /// <summary>�o�b�g�̃O���b�v�ʒu</summary>
    [SerializeField] Transform grip;

    [Header("������艺�̕ϐ��́A�v�������K�v�Ȃ���")]
    /// <summary>�o�b�g�̔����W���̍Œ�l</summary>
    [SerializeField, Range(0f, 0.5f)] float minValueOfRepulsionCoeffiecient;
    /// <summary>�o�b�g��U�������̃C�x���g���΂�</summary>
    [SerializeField] UnityEventWrapperFloat OnSwingAction = default;

    [Header("�f�o�b�N�p")]
    /// <summary>�f�o�b�N���̃o�b�g�̉�]���x</summary>
    [SerializeField] float debugRotatePower = -10;
    /// <summary>�{�[�������������@�������ւ��̂܂܂̑��x�Ŕ�΂�</summary>
    [SerializeField] bool isJustKeepHittingBallBack = false;

    /// <summary>�o�b�g�ɓ��������u�ԂɃt���O�𗧂Ă�</summary>
    private bool onImpact = false;
    /// <summary>1�t���[���O�̒��S�ʒu</summary>
    private Vector3 lastBarycenterPos;
    /// <summary>�w�b�h�X�s�[�h:�o�b�g�̏d�S���O���b�h�𒆐S�Ƃ���~�^�����瓾���鑬�x(�P�ʂ� km/h)</summary>
    private float headSpeed;
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

        transform.RotateAround(grip.position, Vector3.up, debugRotatePower);

        headSpeed = GetHeadSpeed(lastBarycenterPos, barycenter.position);

        barycenterVectorOnImpact = (barycenter.position - lastBarycenterPos).normalized;

        //Debug.Log(barycenterVelocity * 1000);
        //Debug.DrawRay(barycenter.position, barycenterVectorOnImpact, Color.white);

        lastBarycenterPos = barycenter.position;
    }

    /// <summary>
    /// �w�b�h���x�i�o�b�g�̏d�S���O���b�h�𒆐S�Ƃ���~�^�����瓾���鑬�x�j�𓾂�֐�.
    /// </summary>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>
    /// <returns></returns>
    private float GetHeadSpeed(Vector3 lastPos, Vector3 currentPos)
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
        var finalVelocity = radius * barycenterAngularVelocity;
        //m/s �� km/h�ɕϊ�
        finalVelocity = finalVelocity * 3.6f * 90f; // 1/90f �� 1f
        //Debug�p��UI�Ƀw�b�h�X�s�[�h��\������.
        OnSwingAction?.Invoke(finalVelocity);
        return finalVelocity;
    }

    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        if (isJustKeepHittingBallBack)
        {
            rb.velocity = hitObjectInfo.normal * ballSpeed;
            return;
        }
        rb.velocity = hitObjectInfo.normal * BattingPower(hitObjectInfo, ballSpeed);
        Debug.Log("���� :" + BattingPower(hitObjectInfo, ballSpeed));
    }

    private float BattingPower(RaycastHit hitObjectInfo, float ballSpeed)
    {
        //�C���p�N�g�̏u�Ԃ̃o�b�g�̌����ƃ{�[�������������ʂ̖@���x�N�g���̓��ό��ʂɂ��␳�l��������.
        float justMeetRatio = Vector3.Dot(barycenterVectorOnImpact, hitObjectInfo.normal);
        justMeetRatio = Mathf.Clamp(justMeetRatio, minValueOfRepulsionCoeffiecient, 1f);

        //�{�[�������������ꏊ����o�b�g�̏d�S�܂ł̋���������,�����ɉ����ăp���[�ɕ␳�l��������.
        float hitCoreRatio = Vector3.Distance(hitObjectInfo.point, barycenter.position) * 100;
        //�o�b�g�̐c�ɋ߂���1�ɋ߂��l��Ԃ��A�o�b�g�̐c����̋�����17cm�������0��Ԃ�
        hitCoreRatio = Mathf.Clamp01(1 - hitCoreRatio / 17);

        //Debug.Log("���� :" + ballSpeed * e);
        //Debug.Log("���[�����g: " + barycenterVelocity * length);
        // Debug.Log("���� :" +  ballSpeed + headSpeed);
        // Debug.Log("���� :" + (e + length)/2);
        //�ŏI�I�ȃ{�[���ɉ�����͂��v�Z���ʂ�Ԃ��i�����C������)
        return ballSpeed * ((justMeetRatio + hitCoreRatio) / 2) * BaseBallLogic.CoefficientOfRestitution + headSpeed;
    }

}
