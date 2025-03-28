using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class PlayerSettingsManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] TMP_Dropdown collectionFreqDropdown;
    [SerializeField] Toggle realTimeCollectionToggle;
    [SerializeField] Toggle AIToggle;

    public int collectionFrequency = 0;
    public bool realTimeCollection = true;
    public bool AIControl = false;

    private void Start()
    {
        // Load saved settings
        if (PlayerPrefs.HasKey("musicVolume")) { LoadVolume(); }
        else { ChangeVolume(); }

        if (PlayerPrefs.HasKey("collectionFrequency"))
        {
            collectionFrequency = PlayerPrefs.GetInt("collectionFrequency");
            collectionFreqDropdown.value = collectionFreqDropdown.options.FindIndex(option => option.text == collectionFrequency.ToString());
        }

        if (PlayerPrefs.HasKey("realTimeCollection"))
        {
            realTimeCollection = PlayerPrefs.GetInt("realTimeCollection") == 1;
            realTimeCollectionToggle.isOn = realTimeCollection;
        }

        if (PlayerPrefs.HasKey("AIControl"))
        {
            AIControl = PlayerPrefs.GetInt("AIControl") == 1;
            AIToggle.isOn = AIControl;
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

    public void onCollectionFrequencyDropdown()
    {
        int index = collectionFreqDropdown.value;
        collectionFrequency = Convert.ToInt32(collectionFreqDropdown.options[index].text);
        PlayerPrefs.SetInt("collectionFrequency", collectionFrequency);
        Debug.Log("Col Freq: " + collectionFrequency);
    }

    public void onRTCToggle()
    {
        realTimeCollection = realTimeCollectionToggle.isOn;
        PlayerPrefs.SetInt("realTimeCollection", realTimeCollection ? 1 : 0);
    }

    public void onAIControlToggle()
    {
        AIControl = AIToggle.isOn;
        PlayerPrefs.SetInt("AIControl", AIControl ? 1 : 0);
    }
}
