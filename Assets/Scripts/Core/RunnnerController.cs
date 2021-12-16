using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Katsumata
{
    public class RunnnerController : MonoBehaviour
    {
        //先に出ていた人から消えていく（ホームベースに戻る）のでキューで処理する
        List<Runner> m_runners = new List<Runner>();
        [SerializeField] GameObject m_runnerObj;
        [SerializeField] GameObject m_homeBase;
        [SerializeField] GameObject m_base1;
        [SerializeField] GameObject m_base2;
        [SerializeField] GameObject m_base3;
        Dictionary<BaseName, Vector3> m_basePositions = new Dictionary<BaseName, Vector3>();
        [SerializeField] float m_moveSpeed = 1.0f;


        // Start is called before the first frame update
        void Start()
        {
            RunnersInit();
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.Z))
            {
                TestHit(1);
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                TestHit(2);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                TestHit(3);
            }
            else if (Input.GetKeyDown(KeyCode.V))
            {
                TestHit(4);
            }
#endif

        }

        /// <summary>
        /// ランナーやベースの情報を初期化する
        /// </summary>
        private void RunnersInit()
        {
            //走塁先の座標を登録する
            m_basePositions.Add(BaseName.Home, m_homeBase.transform.position);//ホームの座標
            m_basePositions.Add(BaseName.First, m_base1.transform.position);//1塁の座標
            m_basePositions.Add(BaseName.Second, m_base2.transform.position);//2塁の座標
            m_basePositions.Add(BaseName.Third, m_base3.transform.position);//3塁の座標
        }

        void RunnerMove(int hitNum)
        {
            //ランナーインスタンスを追加する
            m_runners.Add(new Runner(BaseName.Home, Instantiate(m_runnerObj)));
            //すべてのランナーを走らせる
            for (int i = 0; i < m_runners.Count; i++)
            {
                for (int j = 0; j < hitNum; j++)
                {
                    var nowBase = m_runners[i].GetBasePosi;
                    if (nowBase == BaseName.Third)
                    {
                        m_runners[i].GetOnBase();//ホームに進む
                        m_runners[i].RegisterRelayPoint(m_basePositions[BaseName.Home]);
                        StartCoroutine(RunningAnim(m_runners[i], BaseName.Home, m_moveSpeed));
                        m_runners.RemoveAt(i);
                        i--;
                        break;
                    }
                    else
                    {
                        nowBase++;
                        m_runners[i].GetOnBase();//次の塁に進む
                        //StartCoroutine(RunningAnim(m_runners[i], nowBase, moveSpeed));
                        m_runners[i].RegisterRelayPoint(m_basePositions[nowBase]);
                        if (j == hitNum - 1)
                        {
                            StartCoroutine(RunningAnim(m_runners[i], nowBase, m_moveSpeed));
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 仮のヒット関数
        /// </summary>
        /// <param name="advanceBases">進塁する数。</param>
        void TestHit(int advanceBases)
        {
            RunnerMove(advanceBases);
            Debug.Log("ランナーの数は : " + m_runners.Count);
        }

        IEnumerator RunningAnim(Runner runner, BaseName nextBase, float speed)
        {
            var tmp = 0.0f;
            var finishPosi = Vector3.zero;
            var nowPosi = runner.GetRunnerObj.transform.position;
            var relayPositions = runner.GetRelayPoints;
            for (int i = 0; i < relayPositions.Count; i++)
            {
                finishPosi = relayPositions[i];
                while (tmp <= 1.0f)
                {
                    runner.GetRunnerObj.transform.position = Vector3.Lerp(nowPosi, finishPosi, tmp);
                    tmp += speed * Time.deltaTime;

                    yield return null;
                }
                nowPosi = runner.GetRunnerObj.transform.position;
                tmp = 0.0f;
            }
            
            runner.GetRunnerObj.transform.position = finishPosi;
            runner.DeleteAllRelayPoint();

            if (nextBase == BaseName.Home)
            {
                Destroy(runner.GetRunnerObj);
            }
            yield break;
        }

    }




    public enum BaseName
    {
        Home, First, Second, Third
    }


    /// <summary>
    /// 走る人
    /// </summary>
    public class Runner
    {
        BaseName m_nowBaseName = BaseName.Home;
        GameObject m_body;
        List<Vector3> m_relayPoints = new List<Vector3>();//中継地点の登録リスト

        public BaseName GetBasePosi
        {
            get
            {
                return m_nowBaseName;
            }
        }

        public GameObject GetRunnerObj
        {
            get
            {
                return m_body;
            }
        }

        public List<Vector3> GetRelayPoints
        {
            get
            {
                return m_relayPoints;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="basePosi"></param>
        public Runner(BaseName basePosi, GameObject body)
        {
            m_nowBaseName = basePosi;
            m_body = body;
        }

        /// <summary>
        /// ランナーが出塁する
        /// </summary>
        /// <param name="totalBases">塁打数</param>
        public void GetOnBase()
        {
            m_nowBaseName++;
        }

        //UniTask TestRunningAnimation(Vector3 nextBasePosi)
        //{
        //    float t = 0.0f;
        //    Vector3.Lerp(m_body.transform.position, nextBasePosi, t);
        //}

        /// <summary>
        /// 中継地点の追加。複数の累を跨いで走るアニメーションを行うため
        /// </summary>
        /// <param name="position"></param>
        public void RegisterRelayPoint(Vector3 position)
        {
            m_relayPoints.Add(position);
        }

        public void DeleteAllRelayPoint()
        {
            m_relayPoints.Clear();
        }
    }
}