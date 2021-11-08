using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ボールに当たったら何かしらの処理を呼ぶゲームオブジェクトに継承するインターファイス
/// ex; バット
/// </summary>
public interface IBallHitObjet
{
    /// <summary>
    /// ボールが当たった時の処理をする関数
    /// </summary>
    /// <param name="rb">ボールオブジェクトのRigdBody</param>
    /// <param name="normal">ヒットしたオブジェクトのMeshの法線ベクトル</param>
    /// <param name="ballSpeed">当たる直前のボールスピード</param>
    void OnHit(Rigidbody rb, Vector3 normal, float ballSpeed);
}

[Serializable]
public class UnityEventWrapper : UnityEvent { }




