using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool loop;
    public AudioSource source;
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private Sound[] soundEffects;
    [SerializeField] private Sound[] music;
    private AudioSource currentMusic;

    private Dictionary<string, AudioSource> SoundEffectsDictionary;
    private Dictionary<string, AudioSource> MusicDictionary;
    private float musicVolume;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void SetVolumeAtStart()
    {
        if(PlayerPrefs.HasKey("MusicVolume"))
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
        else
        {
            musicVolume = 0.5f;
            PlayerPrefs.SetFloat("MusicVolume",musicVolume);
        }

        SetSoundEffectsVolume(PlayerPrefs.GetFloat("SoundVolume"));
        if(!PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("SoundVolume", 0.5f);
        }
        SetSoundEffectsVolume(PlayerPrefs.GetFloat("SoundVolume"));
    }
    

    void Start()
    {
        SoundEffectsDictionary = new Dictionary<string, AudioSource>();
        foreach(Sound s in soundEffects)
        {
            if(!SoundEffectsDictionary.ContainsKey(s.name))
            {
                AudioSource source = this.gameObject.AddComponent<AudioSource>();
                source.clip = s.clip;
                source.loop = s.loop;
                source.playOnAwake = false;
                s.source = source;
                SoundEffectsDictionary.Add(s.name, source);
            }
        }

        MusicDictionary = new Dictionary<string, AudioSource>();
        foreach (Sound s in music)
        {
            if (!MusicDictionary.ContainsKey(s.name))
            {
                AudioSource source = this.gameObject.AddComponent<AudioSource>();
                source.clip = s.clip;
                source.loop = s.loop;
                source.playOnAwake = false;
                s.source = source;
                MusicDictionary.Add(s.name, source);
            }
        }
        //ChangeMusic();
        SetVolumeAtStart();
        SetStartMusic();
    }
    void SetStartMusic()
    {
        SetMusicVolume(musicVolume);
        currentMusic = GetRandomMusic();
        currentMusic.Play();
    }
    public void PlaySound(string name)
    {
        if (!SoundEffectsDictionary.ContainsKey(name))
            return;

        SoundEffectsDictionary[name].Play();
    }
    public void SetSoundEffectsVolume(float vol)
    {
        foreach(KeyValuePair<string,AudioSource> s in SoundEffectsDictionary)
        {
            s.Value.volume = vol;
            //Debug.Log("Changing, targe: " +vol + " real: "+ s.Value.volume);
        }
    }
    public void SetMusicVolume(float vol)
    {
        musicVolume = vol;
        foreach (KeyValuePair<string, AudioSource> s in MusicDictionary)
        {
            s.Value.volume = vol;
            //Debug.Log(s.Value.volume);
            //Debug.Log("Changing, targe: " +vol + " real: "+ s.Value.volume);
        }
    }
    public void ChangeMusic(string n="random")
    {
        if(currentMusic != null)
        {
            StartCoroutine(Fade(currentMusic, 0, 1));
            //currentMusic.Stop();
        }

        AudioSource s;
        if(n=="random")
        {
            s = GetRandomMusic();
        }
        else
        {
            s = MusicDictionary[n];
        }
        s.Play();
        StartCoroutine(Fade(s, musicVolume, 1,false));
        currentMusic = s;
        
    }
    AudioSource GetRandomMusic()
    {
        List<AudioSource> musicList = new List<AudioSource>();
        foreach (KeyValuePair<string, AudioSource> s in MusicDictionary)
        {
            musicList.Add(s.Value);
        }
        int rand = Random.Range(0, musicList.Count);
        if(musicList[rand] == currentMusic)
        {
            return GetRandomMusic();
        }
        else
        {
            return musicList[rand];

        }
    }
    public static IEnumerator Fade(AudioSource audioSource, float targetVolume, float FadeTime,bool shouldDisable = true)
    {
        float startVolume = 0;
        if(targetVolume == 0)
            startVolume = audioSource.volume;
        float vol;
        float t = 0f;
        while (t < 1)
        {
            vol = Mathf.Lerp(startVolume, targetVolume, t);
            t += Time.deltaTime/FadeTime;
            audioSource.volume = vol;
            yield return null;
        }

        audioSource.volume = targetVolume;
        if (audioSource.volume == 0 && shouldDisable)
        {
            audioSource.Stop();
        }
    }
}
