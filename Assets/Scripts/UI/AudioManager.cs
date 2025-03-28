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
