using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq.Expressions;

public class SoundManager : MonoBehaviour
{
    [Range(0,1)]
    public float masterVolume = 1;
    public static SoundManager soundManager;

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0,1)]
        public float volume = 1;
    }

    public Sound[] sounds;
    public List<AudioSource> audioSources;

    void Start()
    {
        if (soundManager == null)
        {
            soundManager = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        
    }
    public void Play(string soundName)
    {
        if (Array.Find(sounds, sound => sound.name == soundName) != null)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == soundName);
            bool played = false;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].clip = sound.clip;
                    audioSources[i].volume = sound.volume * masterVolume;
                    audioSources[i].Play();
                    played = true;
                    break;
                }
            }
            if (!played)
            {
                audioSources.Add(gameObject.AddComponent<AudioSource>());
                Play(soundName);
            }
        } else
        {
            Debug.Log("soundName not identified");
        }
    }
    public void PlayLooping(string soundName)
    {
        if (Array.Find(sounds, sound => sound.name == soundName) != null)
        {
            Sound sound = Array.Find(sounds, sound => sound.name == soundName);
            bool played = false;
            for (int i = 0; i < audioSources.Count; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    audioSources[i].clip = sound.clip;
                    audioSources[i].volume = sound.volume * masterVolume;
                    audioSources[i].loop = true;
                    audioSources[i].Play();
                    played = true;
                    break;
                }
            }
            if (!played)
            {
                audioSources.Add(gameObject.AddComponent<AudioSource>());
                Play(soundName);
            }
        }
        else
        {
            Debug.Log("soundName not identified");
        }
    }
    public void StopAllSound()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].Stop();
        }
    }
}
