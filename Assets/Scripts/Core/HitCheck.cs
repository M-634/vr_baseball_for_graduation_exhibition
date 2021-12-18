using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �s�b�`���[����������A�{�[�������������R���C�_�[�ɂ���Ĕ�����s���N���X.
/// </summary>
[RequireComponent(typeof(BoxCollider))]
public class HitCheck : MonoBehaviour,IBallHitObjet
{
    /// <summary>���̔�����s���I�u�W�F�N�g�Ȃ̂��\�ߌ��߂Ă���</summary>
    [SerializeField] JudgeType judgeType;
    private bool isHit = false;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
        GetComponent<MeshRenderer>().enabled = false;

        BaseBallLogic.Instance.OnThrowBall += () => isHit = false;
    }

    public void OnHit(Rigidbody rb, RaycastHit hit, float ballSpeed)
    {
        if (isHit) return;

        //�z�[���������A��O�i�t�@�[������j
       if(judgeType == JudgeType.HomeRun || judgeType == JudgeType.OffThePremises 
            || judgeType == JudgeType.Catcher || judgeType == JudgeType.Pitcher)
        {
            rb.gameObject.SetActive(false);
        }
        BaseBallLogic.Instance.UpdateJudgeType(judgeType);
    }
}

