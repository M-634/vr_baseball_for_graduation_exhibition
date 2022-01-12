using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// �o�ۂ��Ă��郉���i�[.
/// </summary>
public class Runner
{
    private GameObject runner;
    private float speed;

    public Transform CurrentPos { get; set; }

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="prefabObject"></param>
    public Runner(GameObject prefabObject, float speed)
    {
        runner = prefabObject;
        this.speed = speed;
    }

    /// <summary>
    /// ���̗ۂ܂Ői��
    /// </summary>
    /// <param name="nextPos"></param>
    public void Move(Transform nextPos)
    {
        //���̗ۂ̕����֑̂���������.
        runner.transform.LookAt(nextPos, runner.transform.up);
    }
}

/// <summary>
/// �����i�[���Ǘ�����N���X.
/// </summary>
public class RunnerManager : MonoBehaviour
{
    /// <summary> �����i�[�v���n�u </summary>
    [SerializeField] GameObject m__runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : �e�����ɑΉ������x�[�X</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>�����i�[�̑��x</summary>
    [SerializeField] float moveSpeed = 1f;

    /// <summary>���ݏo�ۂ��Ă��郉���i�[�̃��X�g</summary>
    private List<Runner> currentRunner = new List<Runner>();

    // Start is called before the first frame update
    void Start()
    {
        if (BaseBallLogic.Instance)
        {
            BaseBallLogic.Instance.OnProcessRunner += Instance_OnProcessRunner;
        }
    }


    private UniTask Instance_OnProcessRunner(JudgeType arg)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// �����i�[�𓮂�������������
    /// </summary>
    /// <param name="hitNumber">�q�b�g=1�A�c�[�x�[�X�q�b�g=2�A�X���[�x�[�X�q�b�g=3�A�z�[������=4</param>
    public void MoveRunner(int hitNumber)
    {
        //���ݏo�ۂ��Ă��郉���i�[��悸�͑��点��.
        for (int i = 0; i < currentRunner.Count; i++)
        {

        }
    }
   
}
