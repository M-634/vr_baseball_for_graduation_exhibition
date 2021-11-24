using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

#region Interfaces
/// <summary>
/// ボールに当たったら何かしらの処理を呼ぶゲームオブジェクトに継承するインターファイス
/// ex; バット
/// </summary>
public interface IBallHitObjet
{
    /// <summary>
    /// ボールが当たった時の処理をする関数
    /// </summary>
    void OnHit(Rigidbody rb, RaycastHit hitObjectInfo, float ballSpeed);
}
#endregion

#region EventWrapperClasses
/// <summary>
/// UnityEventのラッパークラス.
/// インスペクター上で関数を登録できる
/// </summary>
[Serializable]
public class UnityEventWrapperDefault : UnityEvent { }

[Serializable]
public class UnityEventWrapperFloat: UnityEvent<float> { }
#endregion

#region siglton
/// <summary>
/// シングルトンパターンを使用するオブジェクに継承させる抽象クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                Type t = typeof(T);

                instance = (T)FindObjectOfType(t);
                if (instance == null)
                {
                    Debug.LogWarning(t + "をアタッチしているGameObjectはありません");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        //他のGameObjectにアタッチされているか調べる
        //アタッチされている場合は破棄する
        if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
#endregion


