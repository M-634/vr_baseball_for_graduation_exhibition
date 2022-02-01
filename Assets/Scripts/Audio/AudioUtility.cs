using UnityEngine;
using System;

//Audio Clipのクラスをまとめたファイル.

[Serializable]
public class SFXClip
{
    [SerializeField] KindOfSFX m_kindOfSFX;
    [SerializeField] AudioClip m_clip;

    public KindOfSFX GetKindOfSFX => m_kindOfSFX;
    public AudioClip GetClip => m_clip;
}

[Serializable]
public class BGMClip
{
    [SerializeField] KindOfBGM m_kindOfBGN;
    [SerializeField] AudioClip m_clip;

    public KindOfBGM GetKindOfBGM => m_kindOfBGN;
    public AudioClip GetClip => m_clip;
}


public enum KindOfSFX
{
    None,HittingByBat, Cather, Hit, Foul, Cheer,FireFlower, PlayBall, UGUI,
    GameStartTrigger, GameOver, StageClear, GameClear
}

public enum KindOfBGM
{
    None,Title, InGame
}


