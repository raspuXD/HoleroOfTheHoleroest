using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMonster : MonoBehaviour
{
    public float shakeDuration = 0f;
    public  float shakeMagnitude = 0.1f;
    public CameraShake camShake;

    public AudioSource source;
     private float nextAudioTime;
    public void Awake()
    {
        // Dynamically find the CameraShake component in the scene
        camShake = FindObjectOfType<CameraShake>();
        if (camShake == null)
        {
            Debug.LogError("No CameraShake component found in the scene! Please add it to your camera.");
        }
        source.Play();
        StartCoroutine(PlayAudioRandomly());
    }

    private IEnumerator PlayAudioRandomly()
    {
        while (true)
        {
            // Wait for a random time between 3 and 7 seconds
            yield return new WaitForSeconds(Random.Range(10f, 15f));

            // Play the audio if it's not already playing
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Cow"))
        {
            CowMilkProducer hoho = collision.GetComponent<CowMilkProducer>();
            if (hoho != null)
            {
                hoho.DIE();
                Destroy(gameObject);
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            camShake.TriggerShake(.1f, .2f);
            Inventory inve = collision.GetComponent<Inventory>();
            if (inve != null)
            {
                inve.Insanity += 7.5f * Time.deltaTime; // Apply insanity over time
            }
        }
    }
}
