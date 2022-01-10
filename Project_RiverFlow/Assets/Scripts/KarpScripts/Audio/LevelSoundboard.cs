using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class LevelSoundboard : MonoBehaviour
{
    [Header("Component")]
    public SoundHandler soundHandler;

    [Header("UI component")]
    public Slider soundSliderMaster;
    public Slider soundSliderUI;
    public Slider soundSliderMusic;
    public Slider soundSliderEffect;

    [Header("Music")]
    public AudioMixerGroup musicGroup;
    [Space(5)]
    public AudioSource musicPlayer;
    public AudioSource backgroundPlayer;
    [Space(15)]
    public AudioClip levelTheme;
    public AudioClip backgroundTheme;

    [Header("Effect")]
    public AudioClip error;

    [Header("UI sound")]
    public AudioMixerGroup uiGroup;
    [Space(15)]
    public AudioClip closeWindow;
    public AudioClip openWindow;
    public AudioClip returnToMainMenu;

    public AudioClip selection;
    //...

    void Start()
    {
        soundHandler = SoundHandler.Instance;

        PlayLevelTheme();
        PlayBackground();

        //Place rightly the slider
        soundSliderMaster.value = PlayerPrefs.GetFloat("masterVolume", 0.5f);
        soundSliderUI.value = PlayerPrefs.GetFloat("uiVolume", 0.5f);
        soundSliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        soundSliderEffect.value = PlayerPrefs.GetFloat("effectsVolume", 0.5f);
    }

    //UI sound
    public void PlayCloseWindow(AudioSource audioSource) { soundHandler.PlaySound(closeWindow, audioSource, uiGroup); }
    public void PlayOpenWindow(AudioSource audioSource) { soundHandler.PlaySound(openWindow, audioSource, uiGroup); }
    public void PlayReturnToMainMenu(AudioSource audioSource) { soundHandler.PlaySound(returnToMainMenu, audioSource, uiGroup); }
    public void PlayErrorSound(AudioSource audioSource) { soundHandler.PlaySound(error, audioSource, uiGroup); }
    public void PlaySelection(AudioSource audioSource) { soundHandler.PlaySound(selection, audioSource, uiGroup); }

    
    //Music
    public void PlayLevelTheme() { soundHandler.PlaySound(levelTheme, musicPlayer, musicGroup); }
    public void PlayBackground() { soundHandler.PlaySound(backgroundTheme, backgroundPlayer, musicGroup); }

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
