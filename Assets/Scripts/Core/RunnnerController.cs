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
        Dictionary<BaseName, Vector3> basePositions = new Dictionary<BaseName, Vector3>();
        [SerializeField] float moveSpeed = 1.0f;
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
            basePositions.Add(BaseName.Home, m_homeBase.transform.position);//ホームの座標
            basePositions.Add(BaseName.First, m_base1.transform.position);//1塁の座標
            basePositions.Add(BaseName.Second, m_base2.transform.position);//2塁の座標
            basePositions.Add(BaseName.Third, m_base3.transform.position);//3塁の座標
        }

        void RunnerMove(int hitNum)
        {
            //キューの末尾にランナーインスタンスを追加する
            m_runners.Add(new Runner(BaseName.Home, Instantiate(m_runnerObj)));
            for (int i = 0; i < hitNum; i++)
            {
                //すべてのランナーを走らせる
                for (int j = 0; j < m_runners.Count; j++)
                {
                    var nowBase = m_runners[j].GetBasePosi;
                    if (nowBase == BaseName.Third)
                    {
                        m_runners[j].RunningBase(basePositions[BaseName.Home]);//ホームに進む
                        StartCoroutine(RunningAnim(m_runners[j], BaseName.Home, moveSpeed));

                        m_runners.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        StartCoroutine(RunningAnim(m_runners[j], ++nowBase, moveSpeed));
                        //m_runners[j].RunningBase(basePositions[++nowBase]);//次の塁に進む
                    }
                }
            }
        }

        /// <summary>
        /// 仮のヒット関数
        /// </summary>
        /// <param name="advancesBases">進塁する数。</param>
        void TestHit(int advancesBases)
        {
            RunnerMove(advancesBases);
            Debug.Log("ランナーの数は : " + m_runners.Count);
        }

        IEnumerator RunningAnim(Runner runner, BaseName nextBase, float speed)
        {
            float tmp = 0.0f;
            Vector3 finishPosi = basePositions[nextBase];
            Vector3 nowPosi = runner.GetRunnerObj.transform.position;
            while (tmp <= 1.0f)
            {
                runner.GetRunnerObj.transform.position = Vector3.Lerp(nowPosi, finishPosi, tmp);
                tmp += speed * Time.deltaTime;

                yield return null;
            }

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

    public struct RunnerBase
    {
        BaseName m_baseName;//ベースの名前
        Vector3 m_position;//座標

        public RunnerBase(BaseName baseName, Vector3 posi)
        {
            m_baseName = baseName;
            m_position = posi;
        }
    }

    /// <summary>
    /// 走る人
    /// </summary>
    public class Runner
    {
        BaseName m_basePosi = BaseName.Home;
        GameObject m_body;

        public BaseName GetBasePosi
        {
            get
            {
                return m_basePosi;
            }
        }

        public GameObject GetRunnerObj
        {
            get
            {
                return m_body;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="basePosi"></param>
        public Runner(BaseName basePosi, GameObject body)
        {
            m_basePosi = basePosi;
            m_body = body;
        }

        /// <summary>
        /// ランナーの走る先
        /// </summary>
        /// <param name="totalBases">塁打数</param>
        public async void RunningBase(Vector3 nextBasePosi)
        {
            m_basePosi++;
            //m_body.transform.position = nextBasePosi;
            //await TestRunningAnimation(nextBasePosi);
        }

        //UniTask TestRunningAnimation(Vector3 nextBasePosi)
        //{
        //    float t = 0.0f;
        //    Vector3.Lerp(m_body.transform.position, nextBasePosi, t);
        //}
    }
}