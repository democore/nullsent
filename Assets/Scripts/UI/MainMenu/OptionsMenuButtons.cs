using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuButtons : MonoBehaviour {

    public GameObject MainMenuPrefab;
    public Slider SFXSlider;
    public Slider MusicSlider;

    private void Awake()
    {
        SFXSlider.value = AudioManager.Instance.SfxVolume;
        MusicSlider.value = AudioManager.Instance.MusicVolume;
    }

    public void Back()
    {
        Instantiate(MainMenuPrefab);
        Destroy(gameObject);
        AudioManager.Instance.PlayEffect("beep");
    }

    public void SetSFXVolume(float volume)
    {  
        AudioManager.Instance.SetSFXVolume(volume);
        AudioManager.Instance.PlayEffect("beep");
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }
}
