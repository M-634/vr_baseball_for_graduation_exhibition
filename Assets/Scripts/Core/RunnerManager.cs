using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;


/// <summary>
/// ランナーを管理するクラス.
/// </summary>
public class RunnerManager : SingletonMonoBehaviour<RunnerManager>
{
    /// <summary> ランナープレハブ </summary>
    [SerializeField] GameObject m_runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float m_moveDuration = 1f;

    /// <summary>現在出塁しているランナーのリスト</summary>
    private List<Runner> m_currentRunner = new List<Runner>();

    public Transform GetHomeBase => m_basePostions[0];

    /// <summary>ホームベースに帰ってきたランナーを数える変数</summary>
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
    /// ランナーを動かす命令を発行する関数.
    /// </summary>
    /// <param name="hitNumber">ヒット=1、ツーベースヒット=2、スリーベースヒット=3、ホームラン=4</param>
    public void MoveRunner(int hitNumber)
    {
        //現在出塁しているランナーを先ずは走らせる.
        if (m_currentRunner.Count > 0)
        {
            foreach (Runner runner in m_currentRunner)
            {
                Moving(hitNumber, runner).Forget();
            }
        }

        //新しく出塁するランナーをインスタンス化させて、ヒット数だけ走らせる.
        Runner newRunner = Instantiate(m_runnerSourcePrefab, m_basePostions[0].position, Quaternion.identity).AddComponent<Runner>();
        Moving(hitNumber, newRunner).Forget();

        //出塁ランナーリストへ追加する.
        m_currentRunner.Add(newRunner);
    }

    /// <summary>
    /// 実際にランナーをヒット数だけ走らせる関数.
    /// </summary>
    /// <param name="hitNumber"></param>
    /// <param name="runner"></param>
    private async UniTask Moving(int hitNumber, Runner runner)
    {
        //ヒット数分、ランナーを移動させる.
        for (int i = 0; i < hitNumber; i++)
        {
            int nextBaseIndex = runner.currentBaseNumber + 1;
            //ホームベースに着いたら、得点処理をしてランナーを削除
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
    /// 引数に指定したランナーを削除する関数.
    /// </summary>
    /// <param name="runner"></param>
    private void DeleteRunner(Runner runner)
    {
        m_currentRunner.Remove(runner);
        Destroy(runner.gameObject);
    }

    /// <summary>
    /// 出塁している全てのランナーを削除する関数.
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


