using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// バットが球を打ち返す時に発生する力学的な運動を制御するクラス.
/// </summary>
public class BatControl : MonoBehaviour, IBallHitObjet
{
    /// <summary>バットの中心位置</summary>
    [SerializeField] Transform barycenter;
    /// <summary>バットのグリップ位置</summary>
    [SerializeField] Transform grip;

    [SerializeField] float debugRotatePower = -10;
    /// <summary>バットに当たった瞬間にフラグを立てる</summary>
    private bool onImpact = false;
    /// <summary>1フレーム前の中心位置</summary>
    private Vector3 lastBarycenterPos;
    /// <summary>バット中心の速度</summary>
    private float barycenterVelocity;
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

        transform.RotateAround(grip.position,Vector3.up, debugRotatePower);

        barycenterVelocity = GetBarycenterOfBatVelocity(lastBarycenterPos, barycenter.position);

        barycenterVectorOnImpact = (barycenter.position - lastBarycenterPos).normalized;

        //Debug.Log(barycenterVelocity * 1000);
        //Debug.DrawRay(barycenter.position, barycenterVectorOnImpact, Color.white);

        lastBarycenterPos = barycenter.position;
    }

    /// <summary>
    /// バットの重心がグリッドを中心とする円運動から得られる速度の大きさを返す関数.
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

        //1フレーム前後のベクトルから角度を求める
        float angle = Mathf.Acos(dot);// cosθ = dot / |a| * |b| :   |a| = |b| = 1
        //角度をラジアンに変換
        float radian = angle * Mathf.PI / 180;
        //角速度を求める
        float barycenterAngularVelocity = radian / Time.fixedDeltaTime; //ω = Δθ / Δt
        //グリップからバットの中心までの距離を測る
        radius = (barycenter.position - grip.position).magnitude;
        //バット中心の速度を求める ; v = rω
        return radius * barycenterAngularVelocity;
    }

    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        rb.velocity = hitObjectInfo.normal * BattingPower(hitObjectInfo, ballSpeed);
        Debug.Log("finalpower :" + BattingPower(hitObjectInfo, ballSpeed));
    }

    private float BattingPower(RaycastHit hitObjectInfo ,float ballSpeed)
    {
        //インパクトの瞬間のバットの向きとボールの向きの内積結果による補正値をかける
        float dir = Vector3.Dot(barycenterVectorOnImpact, hitObjectInfo.normal);
        dir = Mathf.Clamp01(dir);

        //ボールが当たった場所からバットの重心までの距離を求め,距離に応じてパワーに補正値をかける
        float length = Vector3.Distance(hitObjectInfo.point, barycenter.position) * 100;
        //バットの芯に近い程1に近い値を返す、バットの芯からの距離が17cm超えると0を返す
        length = Mathf.Clamp01(1 - length / 17);

        Debug.Log("反発 :" + ballSpeed * dir);
        Debug.Log("モーメント: " + barycenterVelocity * length);
        //最終的なボールに加える力を計算結果を返す（随時修正する)
        return  ballSpeed * dir + barycenterVelocity * length;
    }

}
