using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    public bool useRandomPitch;
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f);
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string whatMusicToPlayInStart = "Theme";

    private int currentMusicIndex = 0;
    private float currentMusicTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicSource == null || sfxSource == null || musicSounds.Length == 0)
        {
            return;
        }

        currentMusicIndex = Array.FindIndex(musicSounds, x => x.name == whatMusicToPlayInStart);
        if (currentMusicIndex == -1) currentMusicIndex = 0;

        PlayMusic(musicSounds[currentMusicIndex].name);
    }

    private void Update()
    {
        if (!musicSource.isPlaying)
        {
            PlayNextMusic();
        }
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogError("Music sound '" + name + "' not found!");
            return;
        }

        musicSource.clip = s.clip;
        musicSource.time = 0f;
        musicSource.pitch = 1f;
        musicSource.Play();
    }

    public void PlayNextMusic()
    {
        currentMusicIndex = (currentMusicIndex + 1) % musicSounds.Length;
        PlayMusic(musicSounds[currentMusicIndex].name);
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null) return;

        if (s.useRandomPitch)
        {
            sfxSource.pitch = UnityEngine.Random.Range(s.pitchRange.x, s.pitchRange.y);
        }
        else
        {
            sfxSource.pitch = 1f;
        }

        sfxSource.PlayOneShot(s.clip, sfxSource.volume);
    }
}
