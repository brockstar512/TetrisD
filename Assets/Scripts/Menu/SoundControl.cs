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
    bool isMuteMusic = false;
    bool isMuteFX = false;

    [SerializeField] AudioClip _soundSample;

    private void Awake()
    {
        soundIcon.onClick.AddListener(ChangeVolumeFX);
        musicIcon.onClick.AddListener(ChangeVolumeMusic);

        //get player prefs
    }

    void ChangeVolumeMusic()
    {
        isMuteMusic = !isMuteMusic;
        musicIcon.image.sprite = isMuteMusic ? soundOff : soundOn;
        float volumeLevel = isMuteMusic ? 0 : .75f;
        SoundManager.Instance.ChangeMatserVolume(volumeLevel);
    }
    void ChangeVolumeFX()
    {
        isMuteFX = !isMuteFX;
        soundIcon.image.sprite = isMuteFX ? soundOff : soundOn;
        float volumeLevel = isMuteFX ? 0 : .75f;
        SoundManager.Instance.ChangeMatserVolume(volumeLevel);
        SoundManager.Instance.PlaySound(_soundSample);
    }

    private void OnDestroy()
    {
        soundIcon.onClick.RemoveAllListeners();
        musicIcon.onClick.RemoveAllListeners();
    }

}
