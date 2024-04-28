using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioMixer _audioMixer;
    Dictionary<Sound.SoundLayer, AudioMixerGroup> _dictionaryAudioGroup;
    public Sound[] sounds;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        LearnDictionary();

        foreach  (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = _dictionaryAudioGroup[s.MixerGroup];
        }
    }

    private void LearnDictionary()
    {
        _dictionaryAudioGroup = new Dictionary<Sound.SoundLayer, AudioMixerGroup>();

        AudioMixerGroup[] audioMixGroup = _audioMixer.FindMatchingGroups("Master");

        foreach (AudioMixerGroup group in audioMixGroup)
        {
            Debug.Log(group.name);

            switch (group.name)
            {
                case "Effects":
                    _dictionaryAudioGroup.Add(Sound.SoundLayer.Efecto, group);
                    break;
                case "Music":
                    _dictionaryAudioGroup.Add(Sound.SoundLayer.Musica, group);
                    break;
            }
        }
    }

    private void Start()
    {
        //AudioManager.Instance.Play("idle");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}

[System.Serializable]
public class Sound
{
    public string name;

    public int AudioOutput;

    public AudioClip clip;

    public SoundLayer MixerGroup;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public enum SoundLayer {Efecto, Musica};

    [HideInInspector]
    public AudioSource source;

}