using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

/// <summary>
/// ランナーを制御するクラス
/// </summary>
public class Runner : MonoBehaviour
{
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    public int currentBaseNumber = 0;

    /// <summary>
    /// 次の塁まで進む
    /// </summary>
    public void Move(Transform nextBase, float duration,UnityAction callBack = null)
    {
        //次の塁の方向へ体を向かせる.
        transform.LookAt(nextBase, transform.up);

        //次の塁へ移動する
        transform.DOMove(nextBase.position, duration)
           .OnComplete(() =>
           {
                //ランナーの位置情報を更新
                transform.position = nextBase.position;
                //ホームベースへ向かせる
                transform.LookAt(GameFlowManager.Instance.GetHomeBase, transform.up);
                //コールバック
                callBack?.Invoke();
           });
    }
}
