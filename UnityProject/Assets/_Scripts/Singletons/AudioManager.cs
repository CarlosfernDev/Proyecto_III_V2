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
    private Coroutine FadeRoutine;

    public List<AudioClip> Music;
    private Dictionary<int, AudioClip> MusicDictionary;
    public AudioSource MusicSource;

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
        LearnMusicDictionary();

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

    public void StartFade( float duration, float targetVolume)
    {
        if (FadeRoutine != null) StopCoroutine(FadeRoutine); 
        FadeRoutine = StartCoroutine(StartFadeRoutine( duration, targetVolume));
    }

    public IEnumerator StartFadeRoutine( float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        _audioMixer.GetFloat("Fade", out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            _audioMixer.SetFloat("Fade", Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }

    public void SetMusic(int Scene)
    {
        if (!MusicDictionary.ContainsKey(Scene))
        {
            MusicSource.clip = null;
            return;
        }

        if (MusicDictionary[Scene] == MusicSource.clip) return;

        MusicSource.clip = MusicDictionary[Scene];
        MusicSource.Stop();
        MusicSource.Play();
    }

    private void LearnMusicDictionary()
    {
        MusicDictionary = new Dictionary<int, AudioClip>();
        MusicDictionary.Add( 1, Music[0]);
        MusicDictionary.Add( 2, Music[0]);

        for(int i = 10; i < 90; i++)
        {
            MusicDictionary.Add(i, Music[2]);
        }

        MusicDictionary.Add(100, Music[1]);
        MusicDictionary.Add(101, Music[1]);
        MusicDictionary.Add(103, Music[1]);
    }

}

[System.Serializable]
public class Sound
{
    public string name;

    //public int AudioOutput;

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