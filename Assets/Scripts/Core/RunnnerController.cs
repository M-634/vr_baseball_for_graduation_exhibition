using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Katsumata
{
    public class RunnnerController : MonoBehaviour
    {
        //��ɏo�Ă����l��������Ă����i�z�[���x�[�X�ɖ߂�j�̂ŃL���[�ŏ�������
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
    /// ����l
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
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="basePosi"></param>
        public Runner(BasePosi basePosi)
        {
            m_basePosi = basePosi;
        }

        /// <summary>
        /// �����i�[�̑����
        /// </summary>
        /// <param name="totalBases">�ۑŐ�</param>
        public void RunningBase()
        {
            m_basePosi++;
            if (m_basePosi == BasePosi.Homerun)//�z�[����
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