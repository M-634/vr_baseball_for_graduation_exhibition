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
    [SerializeField] Transform m_barycenter;
    /// <summary>バットのグリップ位置</summary>
    [SerializeField] Transform m_grip;

    [Header("ここより下の変数は、要調整が必要なもの")]
    /// <summary>バットの反発係数の最低値</summary>
    [SerializeField, Range(0f, 0.5f)] float m_minValueOfRepulsionCoeffiecient;
    /// <summary>バットを振った時のイベントを飛ばす</summary>
    [SerializeField] UnityEventWrapperFloat OnSwingAction = default;

    [Header("デバック用")]
    /// <summary>デバック時のバットの回転速度</summary>
    [SerializeField] float m_debugRotatePower = -10;
    /// <summary>ボールが当たった法線方向へそのままの速度で飛ばす</summary>
    [SerializeField] bool m_isJustKeepHittingBallBack = false;

    /// <summary>バットに当たった瞬間にフラグを立てる</summary>
    private bool m_onImpact = false;
    /// <summary>1フレーム前の中心位置</summary>
    private Vector3 m_lastBarycenterPos;
    /// <summary>ヘッドスピード:バットの重心がグリッドを中心とする円運動から得られる速度(単位は km/h)</summary>
    private float m_headSpeed;
    /// <summary>インパクト時のバット中心のベクトルの向き</summary>
    private Vector3 m_barycenterVectorOnImpact;
    /// <summary>グリップからバットの中心までの長さ（円運動における半径）</summary>
    private float m_radius;

    // Start is called before the first frame update
    void Start()
    {
        m_lastBarycenterPos = m_barycenter.position;
    }


    private void FixedUpdate()
    {

        transform.RotateAround(m_grip.position, Vector3.up, m_debugRotatePower);

        m_headSpeed = GetHeadSpeed(m_lastBarycenterPos, m_barycenter.position);

        m_barycenterVectorOnImpact = (m_barycenter.position - m_lastBarycenterPos).normalized;

        //Debug.Log(barycenterVelocity * 1000);
        //Debug.DrawRay(barycenter.position, barycenterVectorOnImpact, Color.white);

        m_lastBarycenterPos = m_barycenter.position;
    }

    /// <summary>
    /// ヘッド速度（バットの重心がグリッドを中心とする円運動から得られる速度）を得る関数.
    /// </summary>
    /// <param name="lastPos"></param>
    /// <param name="currentPos"></param>
    /// <returns></returns>
    private float GetHeadSpeed(Vector3 lastPos, Vector3 currentPos)
    {
        var lhs = (lastPos - m_grip.position).normalized;
        var rhs = (currentPos - m_grip.position).normalized;

        var dot = Vector3.Dot(lhs, rhs);

        if (dot == 1) return 0f;

        //1フレーム前後のベクトルから角度を求める
        float angle = Mathf.Acos(dot);// cosθ = dot / |a| * |b| :   |a| = |b| = 1
        //角度をラジアンに変換
        float radian = angle * Mathf.PI / 180;
        //角速度を求める
        float barycenterAngularVelocity = radian / Time.fixedDeltaTime; //ω = Δθ / Δt
        //グリップからバットの中心までの距離を測る
        m_radius = (m_barycenter.position - m_grip.position).magnitude;
        //バット中心の速度を求める ; v = rω
        var finalVelocity = m_radius * barycenterAngularVelocity;
        //m/s → km/hに変換
        finalVelocity = finalVelocity * 3.6f * 90f; // 1/90f → 1f
        //Debug用のUIにヘッドスピードを表示する.
        OnSwingAction?.Invoke(finalVelocity);
        return finalVelocity;
    }

    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        if (m_isJustKeepHittingBallBack)
        {
            rb.velocity = hitObjectInfo.normal * ballSpeed;
            return;
        }
        Debug.Log("結果 :" + BattingPower(hitObjectInfo, ballSpeed));
        rb.velocity = hitObjectInfo.normal * BattingPower(hitObjectInfo, ballSpeed);
    }

    private float BattingPower(RaycastHit hitObjectInfo, float ballSpeed)
    {
        //インパクトの瞬間のバットの向きとボールが当たった面の法線ベクトルの内積結果による補正値をかける.
        float justMeetRatio = Vector3.Dot(m_barycenterVectorOnImpact, hitObjectInfo.normal);
        justMeetRatio = Mathf.Clamp(justMeetRatio, m_minValueOfRepulsionCoeffiecient, 1f);

        //ボールが当たった場所からバットの重心までの距離を求め,距離に応じてパワーに補正値をかける.
        float hitCoreRatio = Vector3.Distance(hitObjectInfo.point, m_barycenter.position) * 100;
        //バットの芯に近い程1に近い値を返す、バットの芯からの距離が17cm超えると0を返す
        hitCoreRatio = Mathf.Clamp01(1 - hitCoreRatio / 17);

        //最終的なボールに加える力を計算結果を返す（随時修正する)
        return ballSpeed * ((justMeetRatio + hitCoreRatio) / 2) * GameFlowManager.CoefficientOfRestitution + m_headSpeed;
    }

}
