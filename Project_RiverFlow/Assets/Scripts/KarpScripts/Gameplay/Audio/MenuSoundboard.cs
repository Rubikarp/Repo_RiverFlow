using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuSoundboard : MonoBehaviour
{
    [Header("Component")]
    public SoundHandler soundHandler;

    [Header("UI component")]
    public Slider soundSliderMaster;
    public Slider soundSliderUI;
    public Slider soundSliderMusic;
    public Slider soundSliderEffect;

    [Header("UI sound")]
    public AudioMixerGroup uiGroup;
    [Space(15)]
    public AudioClip closeWindow;
    public AudioClip openWindow;
    public AudioClip lauchLevel;
    public AudioClip selection;
    //...

    [Header("Music")]
    public AudioMixerGroup musicGroup;
    [Space(15)]
    public AudioClip mainTheme;
    [Space(5)]
    public AudioSource themePlayer;

    void Start()
    {
        soundHandler = SoundHandler.Instance;

        PlayTheme();

        //Place rightly the slider
        soundSliderMaster.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        soundSliderUI.value = PlayerPrefs.GetFloat("uiVolume", 0.5f);
        soundSliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        soundSliderEffect.value = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
    }

    //UI sound
    public void PlayCloseWindow(AudioSource audioSource) { soundHandler.PlaySound(closeWindow, audioSource, uiGroup); }
    public void PlayOpenWindow(AudioSource audioSource) { soundHandler.PlaySound(openWindow, audioSource, uiGroup); }
    public void PlayLauchLevel(AudioSource audioSource) { soundHandler.PlaySound(lauchLevel, audioSource, uiGroup); }
    public void PlaySelection(AudioSource audioSource) { soundHandler.PlaySound(selection, audioSource, uiGroup); }

    //Music
    public void PlayTheme() { soundHandler.PlaySound(mainTheme, themePlayer, musicGroup); }

    //Change a specific volume with slider
    public void ChangeMasterVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Master);
    }
    public void ChangeUIVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.UI);
    }
    public void ChangeMusicVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Music);
    }
    public void ChangeEffectVolume(float value)
    {
        soundHandler.ChangeVolume(value, SoundMixerGroup.Effect);
    }
}
