using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Sound/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;

    public AudioMixerGroup mixerGroup;

    public int dublicateCount = 5;

    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0f;
    [Range(0f, 0.5f)]
    public float randomPitch = 0f;

    public bool loop = false;
}
