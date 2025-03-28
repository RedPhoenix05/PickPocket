using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSettingsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;

    private void Start()
    {
        // Load saved settings
        if (PlayerPrefs.HasKey("musicVolume")) { LoadVolume(); }
        else { ChangeVolume(); }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("fullscreen") == 1;
            fullscreenToggle.isOn = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        ChangeVolume();
    }

    public void onFullScreenToggle(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
    }
}
