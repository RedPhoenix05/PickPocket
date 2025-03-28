using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerSettingsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Toggle fullscreenToggle;

    private AudioManager audioManager;

    private void Start()
    {
        // Find the AudioManager object
        audioManager = FindObjectOfType<AudioManager>();

        // Load saved settings
        if (PlayerPrefs.HasKey("musicVolume")) { LoadVolume(); }
        else { ChangeVolume(); }

        if (PlayerPrefs.HasKey("sfxVolume")) { LoadSFXVolume(); }
        else { ChangeSFXVolume(); }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("fullscreen") == 1;
            fullscreenToggle.isOn = isFullScreen;
            Screen.fullScreen = isFullScreen;
        }
    }

    public void ChangeVolume()
    {
        float volume = volumeSlider.value;
        if (audioManager != null)
        {
            audioManager.SetMusicVolume(volume);
        }
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
        ChangeVolume();
    }

    public void ChangeSFXVolume()
    {
        float sfxVolume = sfxVolumeSlider.value;
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        if (audioManager != null)
        {
            audioManager.SetSFXVolume(sfxVolume);
        }
        Debug.Log("SFX Volume: " + sfxVolume);
    }

    private void LoadSFXVolume()
    {
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        ChangeSFXVolume();
    }

    public void onFullScreenToggle(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
    }
}
