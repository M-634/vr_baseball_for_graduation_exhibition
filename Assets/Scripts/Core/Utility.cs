using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//今回のプロジェクトで扱う便利な物をまとめたファイルです

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

[Serializable]
public class UnityEventWrapperSendText : UnityEvent<string, UnityAction> { }

[Serializable]
public class UnityEventWrapperDisplayResult : UnityEvent<Result, UnityAction> { }
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

#region enum
public enum HitZoneType
{
   None = 0, Hit = 1,TwoBaseHit = 2,ThreeBaseHit = 3,HomeRun = 4,Foul = 5,Out = 6,Catcher = 7
}
#endregion

#region Observer
public interface IObservable<T>
{
    void Subscribe(Action<T> action);
}

public interface IObserver<T>
{
    void OnNext(T value);
}

/// <summary>
/// UniRxのSubjectクラスを参考にしました。
/// リアクティブプログラミングを実装する簡単なサンプルです。
/// </summary>
/// <typeparam name="T"></typeparam>
public class Subject<T> : IObservable<T>, IObserver<T>
{
    List<Action<T>> m_observers = new List<Action<T>>();

    public void OnNext(T value)
    {
        foreach (var observer in m_observers)
        {
            observer.Invoke(value);
        }
    }

    public void Subscribe(Action<T> action)
    {
        m_observers.Add(action);
    }
}
#endregion


