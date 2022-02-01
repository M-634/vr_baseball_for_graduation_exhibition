using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ���ʂ�ۑ�����ScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "SoundVolumeData", menuName = "CreateSoundVolumeData")]
public class SoundVolumeSO : ScriptableObject
{
    [Range(0f,1f)]
    public float MasterVolume;
    [Range(0f, 1f)]
    public float BGMVolume;
    [Range(0f, 1f)]
    public float SFXVolume;
}
