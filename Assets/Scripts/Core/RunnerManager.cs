using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;


/// <summary>
/// �����i�[���Ǘ�����N���X.
/// </summary>
public class RunnerManager : SingletonMonoBehaviour<RunnerManager>
{
    /// <summary> �����i�[�v���n�u </summary>
    [SerializeField] GameObject m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : �e�����ɑΉ������x�[�X</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>�����i�[�̑��x</summary>
    [SerializeField] float m_moveDuration = 1f;

    /// <summary>���ݏo�ۂ��Ă��郉���i�[�̃��X�g</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];

    /// <summary>�z�[���x�[�X�ɋA���Ă��������i�[�𐔂���ϐ�</summary>
    int countScore = 0;


    private int Instance_OnProcessRunner(JudgeType type)
    {
        countScore = 0;
        switch (type)
        {
            case JudgeType.Hit:
                MoveRunner(1);
                break;
            case JudgeType.TwoBase:
                MoveRunner(2);
                break;
            case JudgeType.ThreeBase:
                MoveRunner(3);
                break;
            case JudgeType.HomeRun:
                MoveRunner(4);
                break;
        }
        return countScore;
    }

    /// <summary>
    /// �����i�[�𓮂������߂𔭍s����֐�.
    /// </summary>
    /// <param name="hitNumber">�q�b�g=1�A�c�[�x�[�X�q�b�g=2�A�X���[�x�[�X�q�b�g=3�A�z�[������=4</param>
    public void MoveRunner(int hitNumber)
    {
        //���ݏo�ۂ��Ă��郉���i�[��悸�͑��点��.
        if (m_currentRunner.Count > 0)
        {
            foreach (Runner runner in m_currentRunner)
            {
                Moving(hitNumber, runner).Forget();
            }
        }

        //�V�����o�ۂ��郉���i�[���C���X�^���X�������āA�q�b�g���������点��.
        Runner newRunner = Instantiate(m_runnerSourcePrefab, m_basePostions[0].position, Quaternion.identity).AddComponent<Runner>();
        Moving(hitNumber, newRunner).Forget();

        //�o�ۃ����i�[���X�g�֒ǉ�����.
        m_currentRunner.Add(newRunner);
    }

    /// <summary>
    /// ���ۂɃ����i�[���q�b�g���������点��֐�.
    /// </summary>
    /// <param name="hitNumber"></param>
    /// <param name="runner"></param>
    private async UniTask Moving(int hitNumber, Runner runner)
    {
        //�q�b�g�����A�����i�[���ړ�������.
        for (int i = 0; i < hitNumber; i++)
        {
            int nextBaseIndex = runner.currentBaseNumber + 1;
            //�z�[���x�[�X�ɒ�������A���_���������ă����i�[���폜
            if (nextBaseIndex > 3)
            {
                nextBaseIndex = 0;
                runner.Move(m_basePostions[nextBaseIndex], m_moveDuration);
                await UniTask.Delay(TimeSpan.FromSeconds(m_moveDuration));
                DeleteRunner(runner);
                countScore++;
                break;
            }
            else
            {
                runner.Move(m_basePostions[nextBaseIndex], m_moveDuration);
                await UniTask.Delay(TimeSpan.FromSeconds(m_moveDuration));
                runner.currentBaseNumber++;
            }
        }
    }


    /// <summary>
    /// �����Ɏw�肵�������i�[���폜����֐�.
    /// </summary>
    /// <param name="runner"></param>
    private void DeleteRunner(Runner runner)
    {
        m_currentRunner.Remove(runner);
        Destroy(runner.gameObject);
    }

    /// <summary>
    /// �o�ۂ��Ă���S�Ẵ����i�[���폜����֐�.
    /// </summary>
    public void ResetRunner()
    {
        if (m_currentRunner.Count == 0) return;

        foreach (var runner in FindObjectsOfType<Runner>())
        {
            DeleteRunner(runner);
        }

    }
}


