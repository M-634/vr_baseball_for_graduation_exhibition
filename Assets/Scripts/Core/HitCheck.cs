using UnityEngine;


/// <summary>
/// �q�b�g������s���N���X.
/// *�����Ō����q�b�g�́A�����i�[���o�ۂ��邱�Ƃł���.
/// </summary>
public class HitCheck : MonoBehaviour,IBallHitObjet
{
    /// <summary>���̔�����s���I�u�W�F�N�g�Ȃ̂��\�ߌ��߂Ă���</summary>
    [SerializeField] HitType hitType;
    /// <summary>�{�[���Ɉ�x����������A����������t���O</summary>
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

