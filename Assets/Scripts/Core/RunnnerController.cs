//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using Cysharp.Threading.Tasks;
//using System.Threading;

//public class RunnnerController : MonoBehaviour
//{
//    [SerializeField] ScoreManager m_scoreManager;

//    //��ɏo�Ă����l��������Ă����i�z�[���x�[�X�ɖ߂�j�̂ŃL���[�ŏ�������
//    List<Runner> m_runners = new List<Runner>();
//    [SerializeField] GameObject m_runnerObj;
//    [SerializeField] GameObject m_homeBase;
//    [SerializeField] GameObject m_base1;
//    [SerializeField] GameObject m_base2;
//    [SerializeField] GameObject m_base3;
//    Dictionary<BaseName, Vector3> m_basePositions = new Dictionary<BaseName, Vector3>();
//    [SerializeField] float m_moveSpeed = 1.0f;
//    [SerializeField] Animator m_runnerAnimController;

//    // Start is called before the first frame update
//    void Start()
//    {
//        RunnersInit();
//        if (BaseBallLogic.Instance)
//        {
//            BaseBallLogic.Instance.OnProcessRunner += Hit;
//        }

//    }

//    /// <summary>
//    /// �����i�[��x�[�X�̏�������������
//    /// </summary>
//    private void RunnersInit()
//    {
//        //���ې�̍��W��o�^����
//        m_basePositions.Add(BaseName.Home, m_homeBase.transform.position);//�z�[���̍��W
//        m_basePositions.Add(BaseName.First, m_base1.transform.position);//1�ۂ̍��W
//        m_basePositions.Add(BaseName.Second, m_base2.transform.position);//2�ۂ̍��W
//        m_basePositions.Add(BaseName.Third, m_base3.transform.position);//3�ۂ̍��W
//    }

//    /// <summary>
//    /// �����i�[�𓮂����������Ǘ����Ă���
//    /// </summary>
//    /// <param name="hitNum">�q�b�g=1�A�c�[�x�[�X�q�b�g=2�A�X���[�x�[�X�q�b�g=3�A�z�[������=4</param>
//    /// <returns></returns>
//    async UniTask RunnerMove(int hitNum)
//    {
//        if (hitNum <= 0 && hitNum > 4) return;

//        var uniTasks = new List<UniTask>();
//        //�V���ɏo�ۂ���(���o�b�^�[)�����i�[�̃C���X�^���X��ǉ�����
//        m_runners.Add(new Runner(BaseName.Home, Instantiate(m_runnerObj), m_runnerAnimController));
//        //���ׂẴ����i�[�𑖂点��
//        for (int i = 0; i < m_runners.Count; i++)
//        {
//            //�q�b�g�łP��A�c�[�x�[�X�q�b�g�łQ��A�X���[�x�[�X�q�b�g�łR��A�z�[�������łS��J��Ԃ�
//            for (int j = 0; j < hitNum; j++)
//            {
//                var nowBase = m_runners[i].GetBasePosi;
//                if (nowBase == BaseName.Third)
//                {
//                    m_runners[i].ToNextBase();//�z�[���ɐi��
//                    m_runners[i].RegisterRelayPoint(m_basePositions[BaseName.Home]);
//                    uniTasks.Add(RunningAnim(m_runners[i], BaseName.Home, m_moveSpeed));
//                    m_runners.RemoveAt(i);
//                    i--;
//                    break;
//                }
//                else
//                {
//                    nowBase++;
//                    m_runners[i].ToNextBase();//���̗ۂɐi��
//                    m_runners[i].RegisterRelayPoint(m_basePositions[nowBase]);
//                    if (j == hitNum - 1)
//                    {
//                        uniTasks.Add(RunningAnim(m_runners[i], nowBase, m_moveSpeed));
//                    }
//                }
//            }
//        }
//        await UniTask.WhenAll(uniTasks);
//        uniTasks.Clear();
//    }

//    /// <summary>
//    /// �q�b�g���ɌĂ΂��֐�
//    /// </summary>
//    /// <param name="advanceBases"></param>
//    /// <returns></returns>
//    async UniTask Hit(JudgeType judgeType)
//    {
//        int advanceBases = 0;

//        switch (judgeType)
//        {
//            case JudgeType.Hit:
//                advanceBases = 1;
//                break;
//            case JudgeType.TwoBase:
//                advanceBases = 2;
//                break;
//            case JudgeType.ThreeBase:
//                advanceBases = 3;
//                break;
//            case JudgeType.HomeRun:
//                advanceBases = 4;
//                break;
//            default:
//                break;
//        }
//        await RunnerMove(advanceBases);
//        Debug.Log("�����i�[�̏�������! �����i�[�̐��� : " + m_runners.Count);
//    }

//    /// <summary>
//    /// �X���[�A�E�g�̂Ƃ��ɌĂ΂��֐��B���ꂩ��task��ǉ����邩���m��Ȃ��̂�UniTask�ɂ��Ă�
//    /// </summary>
//    /// <returns></returns>
//    async UniTask ResetRunners()
//    {
//        for (int i = 0; i < m_runners.Count; i++)
//        {
//            Destroy(m_runners[i].GetRunnerObj);
//        }
//        Debug.Log("�����i�[�����Z�b�g����");
//    }

//    /// <summary>
//    /// �ړ�����A�j���[�V�����𐧌䂷��
//    /// </summary>
//    /// <param name="runner"></param>
//    /// <param name="nextBase"></param>
//    /// <param name="speed"></param>
//    /// <param name="cancellation_token"></param>
//    /// <returns></returns>
//    async UniTask RunningAnim(Runner runner, BaseName nextBase, float speed, CancellationToken cancellation_token = default)
//    {
//        var moveTime = 0.0f;
//        Vector3 finishPosi;//�ړ���̍��W
//        Vector3 rotatePoint;//�ړ����A�ړ���̌���
//        var nowPosi = runner.GetRunnerObj.transform.position;//���݂̍��W
//        var relayPositions = runner.GetRelayPoints;//���p�n�_�A�ŏI�ړI�n�̍��W���i�[����List

//        float preTime = Time.time;
//        float nowTime;
//        for (int i = 0; i < relayPositions.Count; i++)
//        {
//            finishPosi = relayPositions[i];
//            rotatePoint = finishPosi;
//            rotatePoint.y = nowPosi.y;
//            runner.GetRunnerObj.transform.LookAt(rotatePoint);

//            float movex = nowPosi.x;
//            float movez = nowPosi.z;
//            while (moveTime <= 1.0f)
//            {
//                nowPosi.x = Mathf.Lerp(movex, finishPosi.x, moveTime);
//                nowPosi.z = Mathf.Lerp(movez, finishPosi.z, moveTime);
//                runner.GetRunnerObj.transform.position = nowPosi;
//                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellation_token);
//                nowTime = Time.time;
//                moveTime += speed * (Mathf.Abs(nowTime - preTime));//Time.deltatime�ɕς����̂𐶐����Ă���
//                preTime = nowTime;
//            }
//            moveTime = 0.0f;
//        }

//        runner.DeleteAllRelayPoint();

//        if (nextBase == BaseName.Home)
//        {
//            m_scoreManager.AddScore();
//            Debug.Log("���_�� : " + m_scoreManager.GetScore + "�_");
//            Debug.Log("�N���� : " + m_scoreManager.GetIncome + "�~�I");

//            Destroy(runner.GetRunnerObj);
//        }

//        rotatePoint = m_basePositions[BaseName.Home];
//        rotatePoint.y = nowPosi.y;
//        runner.GetRunnerObj.transform.LookAt(rotatePoint);
//        runner.ReachOnBase();
//    }

//#if UNITY_EDITOR
//    public void HitButton()
//    {
//        Hit(JudgeType.Hit);
//    }

//    public void TwoBaseHitButton()
//    {
//        Hit(JudgeType.TwoBase);
//    }

//    public void ThreeBaseHitButton()
//    {
//        Hit(JudgeType.ThreeBase);
//    }

//    public void HomeRunButton()
//    {
//        Hit(JudgeType.HomeRun);
//    }

//    public void ResetRunnerButton()
//    {
//        ResetRunners();
//    }
//#endif

//}

//public enum BaseName
//{
//    Home, First, Second, Third
//}

//public enum RunnerState
//{
//    Idle, Running
//}


///// <summary>
///// ����l
///// </summary>
//public class Runner
//{
//    /// <summary> �ǂ��̃x�[�X�ɂ��邩 </summary>
//    BaseName m_nowBaseName = BaseName.Home;
//    RunnerState m_runnerState = RunnerState.Idle;
//    GameObject m_body;
//    /// <summary> ���p�n�_�̓o�^���X�g </summary>
//    List<Vector3> m_relayPoints = new List<Vector3>();
//    /// <summary> �����i�[�̃A�j���[�^�[���i�[����B�����Ă��郂�[�V������ҋ@���̃��[�V�����������Ɋi�[���A���p���� </summary>
//    Animator m_runnerAnim;

//    public BaseName GetBasePosi
//    {
//        get
//        {
//            return m_nowBaseName;
//        }
//    }

//    public GameObject GetRunnerObj
//    {
//        get
//        {
//            return m_body;
//        }
//    }

//    public List<Vector3> GetRelayPoints
//    {
//        get
//        {
//            return m_relayPoints;
//        }
//    }

//    public Animator GetAnimator
//    {
//        get
//        {
//            return m_runnerAnim;
//        }
//    }

//    public RunnerState GetRunnerState
//    {
//        get
//        {
//            return m_runnerState;
//        }
//    }

//    /// <summary>
//    /// �R���X�g���N�^�[
//    /// </summary>
//    /// <param name="basePosi"></param>
//    public Runner(BaseName basePosi, GameObject body, Animator animator)
//    {
//        m_nowBaseName = basePosi;
//        m_body = body;
//        m_runnerAnim = animator;
//    }

//    /// <summary>
//    /// �����i�[���o�ۂ���
//    /// </summary>
//    /// <param name="totalBases">�ۑŐ�</param>
//    public void ToNextBase()
//    {
//        m_nowBaseName++;
//        m_runnerState = RunnerState.Running;
//    }

//    public void ReachOnBase()
//    {
//        m_runnerState = RunnerState.Idle;
//    }

//    /// <summary>
//    /// ���p�n�_�̒ǉ��B�����̗݂��ׂ��ő���A�j���[�V�������s������
//    /// </summary>
//    /// <param name="position"></param>
//    public void RegisterRelayPoint(Vector3 position)
//    {
//        m_relayPoints.Add(position);
//    }

//    public void DeleteAllRelayPoint()
//    {
//        m_relayPoints.Clear();
//    }
//}
