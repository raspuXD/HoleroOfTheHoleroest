using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowMilkProducer : MonoBehaviour
{
    public GameObject milkPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 5f; // Time between each milk spawn

    public GameObject lastMilk;
    public GameObject porheloDied;
    private float lastSpawnTime;
    public Inventory inventory;

    public AudioSource source;
    public AudioSource EvilMusic;
    public void Start()
    {
        source.Play();
        PlayAudioRandomly();
    }
    void Update()
    {
        // Only start counting down if there is no milk
        if (lastMilk == null)
        {
            if (Time.time - lastSpawnTime > spawnInterval) // Check if it's time to spawn milk
            {
                SpawnMilk();
                lastSpawnTime = Time.time; // Reset the timer
            }
        }
    }

    private IEnumerator PlayAudioRandomly()
    {
        while (true)
        {
            // Wait for a random time between 3 and 7 seconds
            yield return new WaitForSeconds(Random.Range(5f, 15f));

            // Play the audio if it's not already playing
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
    }
    void SpawnMilk()
    {
        lastMilk = Instantiate(milkPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void DIE()
    {
            //EvilMusic.Play();
            porheloDied.SetActive(true);
            inventory.InsanityIncreaseRate = 5f;
            Destroy(gameObject);
    }
}