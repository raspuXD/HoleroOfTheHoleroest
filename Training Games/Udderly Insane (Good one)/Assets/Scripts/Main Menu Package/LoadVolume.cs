using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadVolume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;

    void Start()
    {
        float theSavedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.2f);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(theSavedSFXVolume) * 30);

        float theSavedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(theSavedMusicVolume) * 30);
    }
}
