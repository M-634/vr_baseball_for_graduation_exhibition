using UnityEngine;

/// <summary>
/// ステージデータのスクリプトタルオブジェクト
/// </summary>
[CreateAssetMenu(fileName = "StageData",menuName ="CreateStageDataBase")]
public class StageData : ScriptableObject 
{
    [SerializeField] Stage[] stages;
    public Stage[] GetStageArray => stages;
}

