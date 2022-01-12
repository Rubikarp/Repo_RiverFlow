using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundAsset
{
    public string name;
    public AudioClip clip;
    [Space(10)]
    [Range(0.0f,1.0f)] public float volume = 1.0f;
    public AudioMixerGroup targetGroup;
}
