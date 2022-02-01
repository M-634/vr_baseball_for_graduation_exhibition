using UnityEngine;


/// <summary>
/// �����ǂ̃]�[���Ƀq�b�g�������`�F�b�N����N���X.
/// </summary>
public class CheckHitZone : MonoBehaviour,IBallHitObjet
{
    /// <summary>���̔�����s���I�u�W�F�N�g�Ȃ̂��\�ߌ��߂Ă���</summary>
    [SerializeField] HitZoneType hitZoneType;
    /// <summary>�`�F�b�N�]�[���ɋ��������������̃C�x���g��o�^����ϐ�</summary>
    [SerializeField] UnityEventWrapperDefault OnHitEvent = default;
    /// <summary>�{�[���Ɉ�x����������A����������t���O</summary>
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

        //�z�[�������A�t�@�[���A�A�E�g�A�L���b�`���[�]�[���ɓ���������{�[���̃A�N�e�B�u�𖳌��ɂ���.
        if((int)hitZoneType >= 4)
        {
            rb.gameObject.SetActive(false);
            OnHitEvent?.Invoke();
        }
        hasChecked = true;
    }
}

