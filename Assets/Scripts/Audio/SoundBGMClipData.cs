using UnityEngine;
using System.Collections.Generic;
using System.Linq;



[CreateAssetMenu(fileName = "SoundBGMData", menuName = "CreateSoundBGMData")]
public class SoundBGMClipData : ScriptableObject
{
    [SerializeField] List<BGMClip> m_soundClipList = new List<BGMClip>();

    public AudioClip GetClip(KindOfBGM kindOfBGM)
    {
        return m_soundClipList.FirstOrDefault(clip => clip.GetKindOfBGM == kindOfBGM).GetClip;
    }
}


