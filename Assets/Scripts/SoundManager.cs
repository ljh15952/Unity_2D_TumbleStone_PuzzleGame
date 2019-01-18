using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static SoundManager instance = null;
    private AudioSource backgroundAudio;
    private AudioSource effectAudio;
    private Dictionary<string,AudioClip> backgrounds;
    private Dictionary<string, AudioClip> effects;

    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject soundObject = new GameObject("SoundManager");
              
                instance = soundObject.AddComponent<SoundManager>();
                instance.backgroundAudio = soundObject.AddComponent<AudioSource>();
                instance.effectAudio = soundObject.AddComponent<AudioSource>();
                instance.LoadFile(ref instance.effects, "Sound/Effect/");
                instance.LoadFile(ref instance.backgrounds, "Sound/Background/");
                DontDestroyOnLoad(soundObject);
            }
            return instance;
        }
    }
    private void LoadFile<T>(ref Dictionary<string, T> a, string path) where T : Object
    {
        a = new Dictionary<string, T>();
        T[] particleSystems = Resources.LoadAll<T>(path);
        foreach (var particle in particleSystems)
        {
            a.Add(particle.name, particle);
        }
    }

    public void PlayEffect(string name)
    {
        effectAudio.PlayOneShot(effects[name]);
    }

    public void PlayBackground(string name)
    {
        backgroundAudio.Stop();
        backgroundAudio.clip = backgrounds[name];
        backgroundAudio.Play();
    }

}
