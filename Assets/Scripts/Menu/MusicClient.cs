using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClient : MonoBehaviour
{
    public SoundManager.TrackList track;

    public void Play()
    {
        SoundManager.Instance.PlaySong(track);
    }


}
