using UnityEngine;


/// <summary>
/// 球がどのゾーンにヒットしたかチェックするクラス.
/// </summary>
public class CheckHitZone : MonoBehaviour,IBallHitObjet
{
    /// <summary>何の判定を行うオブジェクトなのか予め決めておく</summary>
    [SerializeField] HitZoneType hitZoneType;
    /// <summary>チェックゾーンに球が当たった時のイベントを登録する変数</summary>
    [SerializeField] UnityEventWrapperDefault OnHitEvent = default;
    /// <summary>ボールに一度当たったら、判定を消すフラグ</summary>
    private bool hasChecked = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        GameFlowManager.Instance.OnThrowBall += (b) => hasChecked = false;
    }

    public void OnHit(Rigidbody rb, RaycastHit hit, float ballSpeed)
    {
        if (hasChecked) return;

        GameFlowManager.Instance.UpdateHitType(hitZoneType);

        //ホームラン、ファール、アウト、キャッチャーゾーンに当たったらボールのアクティブを無効にする.
        if((int)hitZoneType >= 4)
        {
            rb.gameObject.SetActive(false);
            OnHitEvent?.Invoke();
        }
        hasChecked = true;
    }
}

