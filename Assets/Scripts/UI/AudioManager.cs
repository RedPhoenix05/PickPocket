using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source ---------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("--------- Audio Clip ---------")]
    public AudioClip background;
    public AudioClip ambience;

    public AudioClip click;
    public AudioClip cloths_rustle;
    public AudioClip steps;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();

        // Load music and SFX volume if available
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            SetSFXVolume(PlayerPrefs.GetFloat("sfxVolume"));
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        //Debug.Log("Music Volume: " + volume);
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
        //Debug.Log("SFX Volume: " + volume);
    }

    public void onMouseClick()
    {
        SFXSource.clip = click;
        SFXSource.Play();
    }

    public void onClothsRustle()
    {
        SFXSource.clip = cloths_rustle;
        SFXSource.Play();
    }

    public void onSteps()
    {
        SFXSource.clip = steps;
        SFXSource.Play();
    }
}
