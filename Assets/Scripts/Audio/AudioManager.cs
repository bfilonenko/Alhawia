using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private class SoundData
    {
        public AudioSource[] source;

        public int nextIndex = 0;
    }


    private readonly Dictionary<int, SoundData> soundToSoundData = new();


    public void PlaySound(Sound sound)
    {
        int id = sound.GetInstanceID();

        if (soundToSoundData.TryGetValue(id, out SoundData soundData))
        {
            PlaySoundData(sound, soundData);
        }
        else
        {
            SoundData newSoundData = new();
            newSoundData.source = new AudioSource[sound.dublicateCount];

            for (int i = 0; i < sound.dublicateCount; ++i)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = sound.clip;
                audioSource.outputAudioMixerGroup = sound.mixerGroup;

                audioSource.volume = sound.volume;
                audioSource.pitch = sound.pitch;
                audioSource.loop = sound.loop;

                newSoundData.source[i] = audioSource;
            }

            PlaySoundData(sound, newSoundData);
            soundToSoundData.Add(id, newSoundData);
        }
    }


    private void PlaySoundData(Sound sound, SoundData soundData)
    {
        AudioSource source = soundData.source[soundData.nextIndex];
        source.volume = sound.volume * (1 + Random.Range(-sound.randomVolume, sound.randomVolume) / 2f);
        source.pitch = sound.pitch * (1 + Random.Range(-sound.randomPitch, sound.randomPitch) / 2f);
        source.Play();

        ++soundData.nextIndex;
        if (soundData.nextIndex == sound.dublicateCount)
        {
            soundData.nextIndex = 0;
        }
    }
}
