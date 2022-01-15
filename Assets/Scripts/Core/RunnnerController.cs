//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;
//using Cysharp.Threading.Tasks;
//using System.Threading;

//public class RunnnerController : MonoBehaviour
//{
//    [SerializeField] ScoreManager m_scoreManager;

//    //先に出ていた人から消えていく（ホームベースに戻る）のでキューで処理する
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
//    /// ランナーやベースの情報を初期化する
//    /// </summary>
//    private void RunnersInit()
//    {
//        //走塁先の座標を登録する
//        m_basePositions.Add(BaseName.Home, m_homeBase.transform.position);//ホームの座標
//        m_basePositions.Add(BaseName.First, m_base1.transform.position);//1塁の座標
//        m_basePositions.Add(BaseName.Second, m_base2.transform.position);//2塁の座標
//        m_basePositions.Add(BaseName.Third, m_base3.transform.position);//3塁の座標
//    }

//    /// <summary>
//    /// ランナーを動かす処理を管理している
//    /// </summary>
//    /// <param name="hitNum">ヒット=1、ツーベースヒット=2、スリーベースヒット=3、ホームラン=4</param>
//    /// <returns></returns>
//    async UniTask RunnerMove(int hitNum)
//    {
//        if (hitNum <= 0 && hitNum > 4) return;

//        var uniTasks = new List<UniTask>();
//        //新たに出塁する(元バッター)ランナーのインスタンスを追加する
//        m_runners.Add(new Runner(BaseName.Home, Instantiate(m_runnerObj), m_runnerAnimController));
//        //すべてのランナーを走らせる
//        for (int i = 0; i < m_runners.Count; i++)
//        {
//            //ヒットで１回、ツーベースヒットで２回、スリーベースヒットで３回、ホームランで４回繰り返す
//            for (int j = 0; j < hitNum; j++)
//            {
//                var nowBase = m_runners[i].GetBasePosi;
//                if (nowBase == BaseName.Third)
//                {
//                    m_runners[i].ToNextBase();//ホームに進む
//                    m_runners[i].RegisterRelayPoint(m_basePositions[BaseName.Home]);
//                    uniTasks.Add(RunningAnim(m_runners[i], BaseName.Home, m_moveSpeed));
//                    m_runners.RemoveAt(i);
//                    i--;
//                    break;
//                }
//                else
//                {
//                    nowBase++;
//                    m_runners[i].ToNextBase();//次の塁に進む
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
//    /// ヒット時に呼ばれる関数
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
//        Debug.Log("ランナーの処理完了! ランナーの数は : " + m_runners.Count);
//    }

//    /// <summary>
//    /// スリーアウトのときに呼ばれる関数。これからtaskを追加するかも知れないのでUniTaskにしてる
//    /// </summary>
//    /// <returns></returns>
//    async UniTask ResetRunners()
//    {
//        for (int i = 0; i < m_runners.Count; i++)
//        {
//            Destroy(m_runners[i].GetRunnerObj);
//        }
//        Debug.Log("ランナーをリセットした");
//    }

//    /// <summary>
//    /// 移動するアニメーションを制御する
//    /// </summary>
//    /// <param name="runner"></param>
//    /// <param name="nextBase"></param>
//    /// <param name="speed"></param>
//    /// <param name="cancellation_token"></param>
//    /// <returns></returns>
//    async UniTask RunningAnim(Runner runner, BaseName nextBase, float speed, CancellationToken cancellation_token = default)
//    {
//        var moveTime = 0.0f;
//        Vector3 finishPosi;//移動先の座標
//        Vector3 rotatePoint;//移動中、移動後の向き
//        var nowPosi = runner.GetRunnerObj.transform.position;//現在の座標
//        var relayPositions = runner.GetRelayPoints;//中継地点、最終目的地の座標を格納するList

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
//                moveTime += speed * (Mathf.Abs(nowTime - preTime));//Time.deltatimeに変わるものを生成している
//                preTime = nowTime;
//            }
//            moveTime = 0.0f;
//        }

//        runner.DeleteAllRelayPoint();

//        if (nextBase == BaseName.Home)
//        {
//            m_scoreManager.AddScore();
//            Debug.Log("得点は : " + m_scoreManager.GetScore + "点");
//            Debug.Log("年収は : " + m_scoreManager.GetIncome + "円！");

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
///// 走る人
///// </summary>
//public class Runner
//{
//    /// <summary> どこのベースにいるか </summary>
//    BaseName m_nowBaseName = BaseName.Home;
//    RunnerState m_runnerState = RunnerState.Idle;
//    GameObject m_body;
//    /// <summary> 中継地点の登録リスト </summary>
//    List<Vector3> m_relayPoints = new List<Vector3>();
//    /// <summary> ランナーのアニメーターを格納する。走っているモーションや待機中のモーションをここに格納し、引用する </summary>
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
//    /// コンストラクター
//    /// </summary>
//    /// <param name="basePosi"></param>
//    public Runner(BaseName basePosi, GameObject body, Animator animator)
//    {
//        m_nowBaseName = basePosi;
//        m_body = body;
//        m_runnerAnim = animator;
//    }

//    /// <summary>
//    /// ランナーが出塁する
//    /// </summary>
//    /// <param name="totalBases">塁打数</param>
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
//    /// 中継地点の追加。複数の累を跨いで走るアニメーションを行うため
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
