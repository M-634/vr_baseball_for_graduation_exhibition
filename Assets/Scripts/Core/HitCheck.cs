using UnityEngine;


/// <summary>
/// ヒット判定を行うクラス.
/// *ここで言うヒットは、ランナーが出塁することである.
/// </summary>
public class HitCheck : MonoBehaviour,IBallHitObjet
{
    /// <summary>何の判定を行うオブジェクトなのか予め決めておく</summary>
    [SerializeField] HitType hitType;
    /// <summary>ボールに一度当たったら、判定を消すフラグ</summary>
    private bool hasChecked = false;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;

        GameFlowManager.Instance.OnThrowBall += () => hasChecked = false;
    }

    public void OnHit(Rigidbody rb, RaycastHit hit, float ballSpeed)
    {
        if (hasChecked) return;
        GameFlowManager.Instance.UpdateHitType(hitType);
        hasChecked = true;
    }
}

