using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;                // Name of the sound
    public AudioClip clip;             // Reference to the audio clip
    public bool useRandomPitch;        // Whether the sound should have a random pitch
    public Vector2 pitchRange = new Vector2(0.9f, 1.1f); // Range for random pitch
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public string whatMusicToPlayInStart = "Theme";
    private string currentMusicName;
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
        if (musicSource == null || sfxSource == null)
        {
            return;
        }

        PlayMusic(whatMusicToPlayInStart);
    }

    private void Update()
    {
        if (musicSource.isPlaying)
        {
            currentMusicTime = musicSource.time;
        }
    }

    public void PlayMusic(string name)
    {
        if (currentMusicName == name && musicSource.isPlaying)
        {
            return;
        }

        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogError("Music sound '" + name + "' not found!");
        }
        else
        {
            currentMusicName = name;
            musicSource.clip = s.clip;
            musicSource.time = currentMusicTime;
            musicSource.pitch = 1f;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
        }
        else
        {
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

    public void ChangeMusic(string newMusicName, float fadeDuration, float newQuickly)
    {
        StartCoroutine(FadeOutAndIn(newMusicName, fadeDuration, newQuickly));
    }

    private IEnumerator FadeOutAndIn(string newMusicName, float fadeDuration, float howQuicklyNew)
    {
        // Fade out current music
        if (musicSource.isPlaying)
        {
            float startVolume = musicSource.volume;

            while (musicSource.volume > 0)
            {
                musicSource.volume -= startVolume * Time.deltaTime / fadeDuration;
                yield return null;
            }

            musicSource.Stop();
            musicSource.volume = startVolume;
        }

        // Switch to the new music
        Sound newSound = Array.Find(musicSounds, x => x.name == newMusicName);
        if (newSound == null)
        {
            Debug.LogError("Music sound '" + newMusicName + "' not found!");
            yield break;
        }

        musicSource.clip = newSound.clip;
        musicSource.Play();
        musicSource.volume = 0;

        // Fade in the new music
        while (musicSource.volume < 1)
        {
            musicSource.volume += Time.deltaTime / howQuicklyNew;
            yield return null;
        }

        musicSource.volume = 1;
    }
}
