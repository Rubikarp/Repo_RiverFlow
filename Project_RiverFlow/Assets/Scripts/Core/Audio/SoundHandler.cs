using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using NaughtyAttributes;

public enum SoundMixerGroup 
{ 
    Master = 0,
    Music = 1,
    Effect = 2,
    UI = 3,
}

public class SoundHandler : Singleton<SoundHandler>
{
    [Header("Main Parameters")]
    public AudioMixer mainAudioMixer;

    [Header("AudioGroups")]
    public AudioMixerGroup masterGroup;
    [Space(10)]
    public AudioMixerGroup uiGroup;
    public AudioMixerGroup effectsGroup;
    public AudioMixerGroup musicGroup;

    [Header("StartVolumes")]
    [Range(-80f, 20f), Tooltip("Volume in decibel")] public float masterVolume;
    [Space(10)]
    [Range(-80f, 20f), Tooltip("Volume in decibel")] public float uiVolume;
    [Range(-80f, 20f), Tooltip("Volume in decibel")] public float effectsVolume;
    [Range(-80f, 20f), Tooltip("Volume in decibel")] public float musicVolume;

    [Button]
    void SetVolumes()
    {
        //Set groups volume.
        ChangeVolume(masterVolume, "masterVolume");
        //
        ChangeVolume(uiVolume, "uiVolume");
        ChangeVolume(musicVolume, "musicVolume");
        ChangeVolume(effectsVolume, "effectsVolume");
    }

    public void PlaySound(AudioClip clip, AudioSource source, AudioMixerGroup targetGroup)
    {
        source.outputAudioMixerGroup = targetGroup;
        source.clip = clip;

        source.Play();
    }
    public void PlaySound(SoundAsset sound, AudioSource source)
    {
        source.outputAudioMixerGroup = sound.targetGroup;
        source.PlayOneShot(sound.clip, sound.volume);
    }
    public void StopSound(AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }
    public void PauseSound(AudioSource source)
    {
        if (source.isPlaying)
        {
            source.Pause();
        }
    }
    public void UnPauseSound(AudioSource source)
    {
        if (source.clip != null)
        {
            source.UnPause();
        }
    }
    //
    public void CrossFade(AudioSource source, AudioClip clip, float fadeDuration, bool changeVolume = false, float newVolume = 0)
    {
        Coroutine fade = StartCoroutine(CrossfadeRoutine(source, clip, fadeDuration, changeVolume, newVolume));
    }
    public IEnumerator CrossfadeRoutine(AudioSource source, AudioClip clip, float fadeDuration, bool changeVolume = false, float newVolume = 0)
    {
        float time = 0;
        float currentVolume = source.volume;

        while (time < fadeDuration)
        {
            source.volume = Mathf.Lerp(currentVolume, 0, time / fadeDuration);
            yield return null;
            time += Time.deltaTime;
        }

        source.volume = 0;
        source.clip = clip;
        time = 0;

        if (changeVolume) currentVolume = newVolume;

        while (time < fadeDuration)
        {
            if (newVolume == 0) source.volume = Mathf.Lerp(0, currentVolume, time / fadeDuration);
            else source.volume = Mathf.Lerp(0, newVolume, time / fadeDuration);
            yield return null;
            time += Time.deltaTime;
        }

        source.volume = currentVolume;

        if (!source.isPlaying)
            source.Play();
    }

    //Change a volume
    public void ChangeVolume(float value, SoundMixerGroup targetGroup)
    {
        switch (targetGroup)
        {
            case SoundMixerGroup.Master:
                ChangeVolume(value, "masterVolume");
                break;

            case SoundMixerGroup.UI:
                ChangeVolume(value, "uiVolume");
                break;

            case SoundMixerGroup.Music:
                ChangeVolume(value, "musicVolume");
                break;

            case SoundMixerGroup.Effect:
                ChangeVolume(value, "effectsVolume");
                break;

            default:
                Debug.LogError("Can't find the SoundMixer Group", this);
                break;
        }
    }
    public void ChangeVolume(float value, string targetGroup)
    {
        mainAudioMixer.SetFloat(targetGroup, value != 0 ? Mathf.Log10(value) * 20 : -80);
        PlayerPrefs.SetFloat(targetGroup, value);
    }
    public void ChangePitch(float newPitch,AudioSource audioSource)
    {
        audioSource.pitch = newPitch;
    }
}
