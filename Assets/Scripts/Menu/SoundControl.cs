using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    [SerializeField] Button soundIcon;
    [SerializeField] Button musicIcon;
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOff;


    [SerializeField] AudioClip _soundSample;

    private void Awake()
    {
        soundIcon.onClick.AddListener(ChangeVolumeFX);
        musicIcon.onClick.AddListener(ChangeVolumeMusic);
        musicIcon.image.sprite = DataStore.Instance.playerData.isMusicMuted ? soundOff : soundOn;
        soundIcon.image.sprite = DataStore.Instance.playerData.isFXMuted ? soundOff : soundOn;

        //get player prefs
    }

    void ChangeVolumeMusic()
    {
        DataStore.Instance.playerData.isMusicMuted = !DataStore.Instance.playerData.isMusicMuted;
        musicIcon.image.sprite = DataStore.Instance.playerData.isMusicMuted ? soundOff : soundOn;
        SoundManager.Instance.HandleMusicVolume(DataStore.Instance.playerData.isMusicMuted);
    }
    void ChangeVolumeFX()
    {
        DataStore.Instance.playerData.isFXMuted = !DataStore.Instance.playerData.isFXMuted;
        soundIcon.image.sprite = DataStore.Instance.playerData.isFXMuted ? soundOff : soundOn;
        SoundManager.Instance.HandleSFXVolume(DataStore.Instance.playerData.isFXMuted);
        SoundManager.Instance.PlaySound(_soundSample);
    }

    private void OnDestroy()
    {
        soundIcon.onClick.RemoveAllListeners();
        musicIcon.onClick.RemoveAllListeners();
    }

}
