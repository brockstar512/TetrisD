using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClient : MonoBehaviour
{
    [SerializeField] AudioClip _song;
    
    public void Play()
    {
        SoundManager.Instance.PlaySong(_song, true);
    }


}
