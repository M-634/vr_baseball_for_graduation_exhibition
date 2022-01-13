using UnityEngine;

[CreateAssetMenu(fileName = "StageData",menuName ="CreateStageDataBase")]
public class StageData : ScriptableObject 
{
    [SerializeField] Stage[] stages;

    public int currentStageNumber = 0;

    public Stage CurrentStageData => stages[currentStageNumber];

    public bool IsStageClera => currentStageNumber >= stages[currentStageNumber].clearHitNumer;

    public void Init()
    {
        foreach (var s in stages)
        {
            s.ballLeftNumber = s.ballNumber;
        }
        currentStageNumber = 0;
    }
}

