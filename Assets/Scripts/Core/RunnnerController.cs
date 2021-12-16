using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Katsumata
{
    public class RunnnerController : MonoBehaviour
    {
        //先に出ていた人から消えていく（ホームベースに戻る）のでキューで処理する
        Queue<Runner> runnners = new Queue<Runner>();


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR

            if (Input.GetKeyDown(KeyCode.Z))
            {
                for (int i = 0; i < 2; i++)
                {

                }
                foreach (var runnner in runnners)
                {
                    runnner.RunningBase();
                }
                runnners.Enqueue(new Runner(BasePosi.Home));
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {

            }
            else if (Input.GetKeyDown(KeyCode.C))
            {

            }
            else if (Input.GetKeyDown(KeyCode.V))
            {

            }
#endif

        }
    }


    public enum BasePosi
    {
        Home, First, Second, Third, Homerun
    }

    /// <summary>
    /// 走る人
    /// </summary>
    public class Runner
    {
        BasePosi m_basePosi = BasePosi.Home;

        public BasePosi GetBasePosi
        {
            get
            {
                return m_basePosi;
            }
        }

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="basePosi"></param>
        public Runner(BasePosi basePosi)
        {
            m_basePosi = basePosi;
        }

        /// <summary>
        /// ランナーの走る先
        /// </summary>
        /// <param name="totalBases">塁打数</param>
        public void RunningBase()
        {
            m_basePosi++;
            if (m_basePosi == BasePosi.Homerun)//ホームに
            {
                return;
            }
            else
            {
                m_basePosi += 1;
            }

        }
    }
}