using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private string song;
    [SerializeField] private AudioMixerGroup group;
    [SerializeField] private List<Music> allMusic;
    
    public static MusicManager instance;

    [HideInInspector] public Music musicPlaying = null;

    public static float currentPoint = 0;
    public bool DoneLoading;

    private string prevSongName = "";

    void Start()
    {
        DoneLoading = false;
        DontDestroyOnLoad(gameObject);
        if (Globals.MusicManager != null)
        {
            DoneLoading = true;
            Destroy(gameObject);
            return;
        }

        Globals.MusicManager = this;
        LoadSongs();
        
        PlayRandom();
    }
    
    public void LoadSongs() {
        Dictionary<string, ArrayList> musicDict = new Dictionary<string, ArrayList>();
        TextAsset tsvHandler = Addressables.LoadAssetAsync<TextAsset>("Assets/Audio/Music Data.tsv").WaitForCompletion();

        if (tsvHandler == null)
        {
            Debug.LogError("Failed to load music data");
            return;
        }
        
        musicDict = Globals.LoadTSV(tsvHandler);

        foreach(KeyValuePair<string, ArrayList> entry in musicDict) {
            if (entry.Key == "" || entry.Key[0] == '#' || (string) entry.Value[1] == "Intro Point") continue;
            Music music = new Music();
                
            music.fileName = entry.Key;
            music.songName = Convert.ToString(entry.Value[0]);
            
            try
            {
                music.loopStart = Convert.ToSingle(entry.Value[1]);
                music.loopEnd = Convert.ToSingle(entry.Value[2]);
            }
            catch
            {
                music.redirect = Convert.ToString(entry.Value[3]);
            }

            if (music.redirect == null)
            {
                String path = "Assets/Music/" + music.fileName + ".wav";

                AudioClip clipHandler = Addressables.LoadAssetAsync<AudioClip>(path).WaitForCompletion();

                if (clipHandler != null)
                {
                    music.source = gameObject.AddComponent<AudioSource>();
                    music.source.clip = clipHandler;
                    music.source.pitch = music.pitch;
                    music.source.loop = true;
                    music.source.outputAudioMixerGroup = group;
                }
                else
                {
                    Debug.LogError("Failed to load song: " + music.fileName);
                }
            }
            
            allMusic.Add(music);
        }

        DoneLoading = true;
    }

    public void setPoint()
    {
        currentPoint = musicPlaying.source.time;
    }

    public void goToPoint()
    {
        musicPlaying.source.time = currentPoint;
    }
    
    public void Stop(Music song=null)
    {
        if (song == null) song = musicPlaying;
        
        try { song?.source.Stop(); }
        catch {}

        if (song == musicPlaying) musicPlaying = null;
    }

    public Music PlayRandom()
    {
        System.Random rand = new System.Random();

        string s = allMusic.ElementAt(rand.Next(0, allMusic.Count)).fileName;

        return Play(s);
    }

    public Music Play(string name, bool followSoundtrack=true)
    {
        Music s = null;
        if (followSoundtrack) s = allMusic.Find(x => x.fileName == name + $"_{Globals.Soundtrack.ToString()}");
        
        if (s == null)
        {
            s = allMusic.Find(x => x.fileName == name);
            if (s == null) return null;
        }

        if (!string.IsNullOrEmpty(s.redirect)) return Play(s.redirect, false);
        Debug.Log(s.songName);
        return Play(s);
    }
    
    public Music Play (Music s)
    {
        if (musicPlaying == s && Math.Abs(musicPlaying.source.volume - 1) < 0.1 && musicPlaying.source.isPlaying)
        {
            return musicPlaying;
        }

        try
        {
            if (musicPlaying.source.isPlaying)
            {
                FadeOut();
            }
        } catch {}
        
        if (prevSongName != s.fileName) currentPoint = 0f;
        
        musicPlaying = s;

        s.source.volume = 1;
        s.source.time = 0;
        s.source.Play();

        prevSongName = s.fileName;

        return s;
    }

    private void FixedUpdate()
    {
        if (musicPlaying == null) return;
        if (musicPlaying.source == null) return;
        
        if (musicPlaying.fileName != "")
        {
            if (musicPlaying.source.time > musicPlaying.loopEnd && musicPlaying.loopEnd > 0)
            {
                musicPlaying.source.time -= (musicPlaying.loopEnd - musicPlaying.loopStart);
            }
        }
    }

    public void FadeOut(float length=0.1f)
    {
        try
        {
            setPoint();
        }
        catch
        {
            return;
        }

        StartCoroutine(FadeTo(length, 0, musicPlaying));
    }
    
    public void FadeIn(float length=0.1f)
    {
        musicPlaying.source.volume = 0f;
        musicPlaying.source.Play();
        goToPoint();
        StartCoroutine(FadeTo(length, 1, musicPlaying));
    }
    
    public IEnumerator FadeTo(float duration, float targetVolume, Music audioSource=null)
    {
        if (audioSource == null)
        {
            audioSource = musicPlaying;
        }
        
        float currentTime = 0;
        float start = audioSource.source.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.source.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        audioSource.source.volume = targetVolume;

        if (audioSource.source.volume <= 0.1)
        {
            Stop(audioSource);
        }
    }

    public void SetLowpass(float duration, float targetValue)
    {
        StartCoroutine(FadeLowpassFilter(duration, targetValue));
    }

    public IEnumerator FadeLowpassFilter(float duration, float targetValue)
    {
        float currentTime = 0;
        float start;
        group.audioMixer.GetFloat("Music Lowpass", out start);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            @group.audioMixer.SetFloat("Music Lowpass", Mathf.Lerp(start, targetValue, currentTime / duration));
            yield return null;
        }

        @group.audioMixer.SetFloat("Music Lowpass", targetValue);
    }

    public Music GetMusicPlaying() {
        return musicPlaying;
    }

    public void setMusicPlaying(Music music)
    {
        musicPlaying = music;
    }
}
