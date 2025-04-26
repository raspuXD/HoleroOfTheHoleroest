using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    public GameObject cowMelee, cowRanged;
    public Transform[] spawnPoints;
    public float minTimeBetweenSpawns = 1f;
    public float maxTimeBetweenSpawns = 3f;

    public Transform player;

    private bool spawnMeleeNext = true;

    private List<GameObject> spawnedCows = new List<GameObject>(); // List to keep track of spawned cows

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(minTimeBetweenSpawns);
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true) // Keep spawning indefinitely
        {
            // Randomly select a spawn point from the array
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            // Determine which cow to spawn based on the flag
            GameObject objectToSpawn = spawnMeleeNext ? cowMelee : cowRanged;

            // Spawn the object at the chosen point
            GameObject spawnedCow = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
            Enemy_Nav cowInsta = spawnedCow.GetComponent<Enemy_Nav>();
            cowInsta.playerRef = player;

            // Add the spawned cow to the list
            spawnedCows.Add(spawnedCow);

            // Toggle the flag to alternate between melee and ranged
            spawnMeleeNext = !spawnMeleeNext;

            // Wait for the next spawn
            float spawnDelay = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void DestroyAllCows()
    {
        foreach (GameObject cow in spawnedCows)
        {
            if (cow != null)
            {
                Destroy(cow); // Destroy the cow
            }
        }
        spawnedCows.Clear(); // Clear the list after destroying all cows
    }
}
