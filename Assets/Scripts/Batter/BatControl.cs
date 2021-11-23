using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バットが球を打ち返す時に発生する力学的な運動を制御するクラス.
/// </summary>
public class BatControl : MonoBehaviour, IBallHitObjet
{

    [Header("各Transfomをセットする")]
    /// <summary>バットの中心位置</summary>
    [SerializeField] Transform barycenter;
    /// <summary>バットのグリップ位置</summary>
    [SerializeField] Transform grip;

    [Header("ここより下の変数は、要調整が必要なもの")]
    /// <summary>バットの反発係数の最低値</summary>
    [SerializeField, Range(0f, 0.5f)] float minValueOfRepulsionCoeffiecient;
    /// <summary>バットを振った時のイベントを飛ばす</summary>
    [SerializeField] UnityEventWrapperFloat OnSwingAction = default;

    [Header("デバック用")]
    /// <summary>デバック時のバットの回転速度</summary>
    [SerializeField] float debugRotatePower = -10;
    /// <summary>ボールが当たった法線方向へそのままの速度で飛ばす</summary>
    [SerializeField] bool isJustKeepHittingBallBack = false;

    /// <summary>バットに当たった瞬間にフラグを立てる</summary>
    private bool onImpact = false;
    /// <summary>1フレーム前の中心位置</summary>
    private Vector3 lastBarycenterPos;
    /// <summary>ヘッドスピード:バットの重心がグリッドを中心とする円運動から得られる速度(単位は km/h)</summary>
    private float headSpeed;
    /// <summary>インパクト時のバット中心のベクトルの向き</summary>
    private Vector3 barycenterVectorOnImpact;
    /// <summary>グリップからバットの中心までの長さ（円運動における半径）</summary>
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
    /// ヘッド速度（バットの重心がグリッドを中心とする円運動から得られる速度）を得る関数.
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

        //1フレーム前後のベクトルから角度を求める
        float angle = Mathf.Acos(dot);// cosθ = dot / |a| * |b| :   |a| = |b| = 1
        //角度をラジアンに変換
        float radian = angle * Mathf.PI / 180;
        //角速度を求める
        float barycenterAngularVelocity = radian / Time.fixedDeltaTime; //ω = Δθ / Δt
        //グリップからバットの中心までの距離を測る
        radius = (barycenter.position - grip.position).magnitude;
        //バット中心の速度を求める ; v = rω
        var finalVelocity = radius * barycenterAngularVelocity;
        //m/s → km/hに変換
        finalVelocity = finalVelocity * 3.6f * 90f; // 1/90f → 1f
        //Debug用のUIにヘッドスピードを表示する.
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
        Debug.Log("結果 :" + BattingPower(hitObjectInfo, ballSpeed));
    }

    private float BattingPower(RaycastHit hitObjectInfo, float ballSpeed)
    {
        //インパクトの瞬間のバットの向きとボールが当たった面の法線ベクトルの内積結果による補正値をかける.
        float justMeetRatio = Vector3.Dot(barycenterVectorOnImpact, hitObjectInfo.normal);
        justMeetRatio = Mathf.Clamp(justMeetRatio, minValueOfRepulsionCoeffiecient, 1f);

        //ボールが当たった場所からバットの重心までの距離を求め,距離に応じてパワーに補正値をかける.
        float hitCoreRatio = Vector3.Distance(hitObjectInfo.point, barycenter.position) * 100;
        //バットの芯に近い程1に近い値を返す、バットの芯からの距離が17cm超えると0を返す
        hitCoreRatio = Mathf.Clamp01(1 - hitCoreRatio / 17);

        //Debug.Log("反発 :" + ballSpeed * e);
        //Debug.Log("モーメント: " + barycenterVelocity * length);
        // Debug.Log("合力 :" +  ballSpeed + headSpeed);
        // Debug.Log("反発 :" + (e + length)/2);
        //最終的なボールに加える力を計算結果を返す（随時修正する)
        return ballSpeed * ((justMeetRatio + hitCoreRatio) / 2) * BaseBallLogic.CoefficientOfRestitution + headSpeed;
    }

}
