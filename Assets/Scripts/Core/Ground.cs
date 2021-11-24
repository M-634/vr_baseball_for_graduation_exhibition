using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 打球のヒット抜けを防止するクラス.
/// 地面オブジェットにアッタッチしてください.
/// </summary>
public class Ground : MonoBehaviour, IBallHitObjet
{
    [SerializeField] bool doHitCheack = true;
    public void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed)
    {
        if (!doHitCheack) return;

        //位置補正
        var tempPos = rb.transform.position;
        tempPos.y = hitObjectInfo.point.y + 0.05f;
        rb.transform.position = tempPos;

        //速度変換
        //world座標y軸上向きの弾性衝突
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * -1f * BaseBallLogic.CoefficientOfRestitution, rb.velocity.z);
    }
}
