using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// 出塁しているランナー.
/// </summary>
public class Runner
{
    private GameObject runner;
    private float speed;

    public Transform CurrentPos { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="prefabObject"></param>
    public Runner(GameObject prefabObject, float speed)
    {
        runner = prefabObject;
        this.speed = speed;
    }

    /// <summary>
    /// 次の塁まで進む
    /// </summary>
    /// <param name="nextPos"></param>
    public void Move(Transform nextPos)
    {
        //次の塁の方向へ体を向かせる.
        runner.transform.LookAt(nextPos, runner.transform.up);
    }
}

/// <summary>
/// ランナーを管理するクラス.
/// </summary>
public class RunnerManager : MonoBehaviour
{
    /// <summary> ランナープレハブ </summary>
    [SerializeField] GameObject m__runnerSourcePrefab = default;
    /// <summary>0 : homebase, 1,2,3 : 各数字に対応したベース</summary>
    [SerializeField] Transform[] m_basePostions;
    /// <summary>ランナーの速度</summary>
    [SerializeField] float moveSpeed = 1f;

    /// <summary>現在出塁しているランナーのリスト</summary>
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
    /// ランナーを動かす処理をする
    /// </summary>
    /// <param name="hitNumber">ヒット=1、ツーベースヒット=2、スリーベースヒット=3、ホームラン=4</param>
    public void MoveRunner(int hitNumber)
    {
        //現在出塁しているランナーを先ずは走らせる.
        for (int i = 0; i < currentRunner.Count; i++)
        {

        }
    }
   
}
