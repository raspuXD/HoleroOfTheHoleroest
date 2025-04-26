using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayClickEffect : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;   // Reference to your AudioMixer
    [SerializeField] AudioClip soundEffect;   // The sound effect to play
    [SerializeField] string sfxVolumeParameter = "SFXVolume"; // Parameter name in the AudioMixer for SFX volume
    private AudioSource audioSource;

    void Start()
    {
        // Add an AudioSource component to this GameObject if not already present
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Ensure the AudioSource uses the AudioMixer's SFX group
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; // Replace "SFX" with your group name
    }

    void Update()
    {
        // Replace KeyCode.Space with the key you want to use
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlaySound();
        }
    }

    void PlaySound()
    {
        if (soundEffect != null)
        {
            audioSource.PlayOneShot(soundEffect);
        }
        else
        {
            Debug.LogWarning("No sound effect assigned.");
        }
    }
}
