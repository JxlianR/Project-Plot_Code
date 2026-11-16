using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    
    [SerializeField]
    private Sound[] sounds;

    private readonly Dictionary<string, Sound> soundLookup = new Dictionary<string, Sound>();

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else Instance = this;
        
        DontDestroyOnLoad(this);
        
        foreach (Sound sound in sounds)
        {
            if (soundLookup.ContainsKey(sound.name))
            {
                Debug.LogWarning($"Duplicate sound name detected: {sound.name}");
                continue;
            }
            
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            
            soundLookup.Add(sound.name, sound);
        }
    }

    public void Play(string soundName)
    {
        Sound sound = FindSound(soundName);
        sound?.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound sound = FindSound(soundName);
        sound?.source.Stop();
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
            sound.source.Stop();
    }

    private Sound FindSound(string soundName)
    {
        if (!soundLookup.TryGetValue(soundName, out Sound sound))
        {
            Debug.LogWarning($"Audio clip with name '{soundName}' has not been found!");
            return null;
        }

        return sound;
    }
}
