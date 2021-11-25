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

    public void OnHit(Rigidbody rb, RaycastHit hit, float ballSpeed)
    {
        //ホームランか、場外（ファール判定）
       if(judgeType == JudgeType.HomeRun || judgeType == JudgeType.OffThePremises 
            || judgeType == JudgeType.Catcher || judgeType == JudgeType.Pitcher)
        {
            rb.gameObject.SetActive(false);
        }
        BaseBallLogic.Instance.UpdateJudgeType(judgeType);
    }
}

