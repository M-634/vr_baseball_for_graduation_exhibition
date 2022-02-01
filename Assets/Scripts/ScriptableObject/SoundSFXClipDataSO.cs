using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "SoundSFXData", menuName = "CreateSoundSFXData")]
public class SoundSFXClipDataSO: ScriptableObject
{
    [SerializeField] List<SFXClip> m_soundClipList = new List<SFXClip>();

    public AudioClip GetClip(KindOfSFX kindOfSFX)
    {
        return m_soundClipList.FirstOrDefault(clip => clip.GetKindOfSFX == kindOfSFX).GetClip;
    }
}



