using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] AudioSource _effectsSource;
    public List<AudioSource> MusicPlayers;
    public enum TrackList
    {
        Menu,
        Game,
        Multiplayer
    }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);

        }
    }

    public void PlaySound(AudioClip clip)
    {
        _effectsSource.PlayOneShot(clip);
    }

    public void PlaySong(TrackList track)
    {
        StopMusic();
        MusicPlayers[(int)track].Play();

    }

    public void StopMusic()
    {
        for (int i = 0; i < MusicPlayers.Count; i++)
        {
            MusicPlayers[i].Stop();
        }
    }


    public void HandleSFXVolume(bool isMute)
    {
        float volumeLevel = isMute ? 0 : .75f;
        _effectsSource.volume = volumeLevel;
    }

    public void HandleMusicVolume(bool isMute)
    {
        float volumeLevel = isMute ? 0 : .75f;

        for (int i = 0; i < MusicPlayers.Count; i++)
        {
            MusicPlayers[i].volume = volumeLevel;
        }
    }
}
