using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ピッチャーが投げた後、ボールが当たったコライダーによって判定を行うクラス.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class HitCheck : MonoBehaviour,IBallHitObjet
{
    /// <summary>何の判定を行うオブジェクトなのか予め決めておく</summary>
    [SerializeField] JudgeType judgeType;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void OnHit(Rigidbody rb, Vector3 normal, float ballSpeed)
    {
        BaseBallLogicEventManager.Instance.SendMessage(judgeType);
    }
}

