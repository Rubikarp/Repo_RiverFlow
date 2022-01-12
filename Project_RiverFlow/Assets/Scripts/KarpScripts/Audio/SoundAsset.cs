using UnityEngine;
using UnityEngine.Audio;

public class SoundAsset
{
    public AudioClip sound;
    [Space(10)]
    [Range(0.0f,1.0f)] public float volume;
    public AudioMixerGroup targetGroup;
}
