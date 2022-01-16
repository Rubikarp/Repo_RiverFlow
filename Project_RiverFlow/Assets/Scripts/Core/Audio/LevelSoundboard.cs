using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class LevelSoundboard : Singleton<LevelSoundboard>
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
    public AudioMixerGroup FXGroup;
    [Space(5)]
    public AudioSource scoreEffectSource;
    public AudioSource plantEffectSource;
    public AudioSource SpawnSource;
    public AudioSource itemEffectSource;
    public AudioSource irrigatedSource;
    public AudioSource eraseSource;
    [Header("Dig")]
    public AudioSource diggingSource;
    public float digPitch = 1;
    public float minPitchDig;
    public float maxPitchDig;
    public float digPitchIncrement;
    [Header("Grow")]
    public AudioSource growingSource;
    public float minPitchGrow;
    public float maxPitchGrow;


    //...
    //attention au dela de 32 call simultannée ça va plus du tout et le son coupe
    //https://answers.unity.com/questions/1192900/playing-many-audioclips-with-playoneshot-causes-al.html
    [Space(15)]
    public List<SoundAsset> soundEffectLib = new List<SoundAsset>();

    [Header("UI sound")]
    public AudioMixerGroup uiGroup;
    [Space(5)]
    public AudioSource timeUISource;
    public AudioSource modesUISource;
    public AudioSource menuTravellingUISource;
    public AudioSource rewardUISource;
    //...
    [Space(15)]
    public List<SoundAsset> soundUILib = new List<SoundAsset>();


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

    //Music
    public void PlayLevelTheme() { soundHandler.PlaySound(levelTheme, musicPlayer, musicGroup); }
    public void PlayBackground() { soundHandler.PlaySound(backgroundTheme, backgroundPlayer, musicGroup); }

    //SFX
    public void PlayScoreEffectSound(SoundAsset sound)
    {
        soundHandler.PlaySound(sound, scoreEffectSource);
    }
    public void PlayScoreEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, scoreEffectSource);
    }
    public void PlaySpawnEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, SpawnSource);
    }
    //grow effect
    public void PlayGrowEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, growingSource);
    }
    public void ChangeGrowPitch()
    {
        SoundHandler.Instance.ChangePitch(Random.Range(minPitchGrow,maxPitchGrow),growingSource);
    }
    //digging
    public void PlayDigEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, diggingSource);
        
    }
    public void ChangePitchDig()
    {
        SoundHandler.Instance.ChangePitch(digPitch, diggingSource);
    }
    public void UpPitchDig()
    {
        digPitch = Mathf.Clamp(digPitch + digPitchIncrement, minPitchDig, maxPitchDig);
    }
    public void InitializePitchDig()
    {
        digPitch = minPitchDig;
    }
    //Digging
    public void PlayEraseEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, eraseSource);
    }
    public void PlayIrrigatedEffectSound(string name)//alternative si soucis avec unity event
    {
        SoundAsset sound = soundEffectLib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, diggingSource);
    }
    //...

    //UI sound
    public void PlayTimeUISound(string name)
    {
        SoundAsset sound = soundUILib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, timeUISource);
    }
    public void PlayModeUISound(string name)
    {
        SoundAsset sound = soundUILib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, modesUISource);
    }
    public void PlayRewardUISound(string name)
    {
        SoundAsset sound = soundUILib.Find(sound => sound.name == name);
        soundHandler.PlaySound(sound, rewardUISource);
    }
    //...

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
