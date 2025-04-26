using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform[] spawnPoints;
    public float minTimeBetweenSpawns = 1f;
    public float maxTimeBetweenSpawns = 3f;


    public Transform player, cow;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(minTimeBetweenSpawns);
        SpawnObject();
    }

    private void SpawnObject()
    {
        // Randomly select a spawn point from the array
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomIndex];

        // Spawn the object at the chosen point
        GameObject cowsas = Instantiate(objectToSpawn, spawnPoint.position, Quaternion.identity);
        Enemy_Nav cowInsta = cowsas.GetComponent<Enemy_Nav>();
        cowInsta.playerRef = player;
        cowInsta.cowRef = cow;

        // Schedule the next spawn
        float spawnDelay = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
        Invoke("SpawnObject", spawnDelay);
    }
}
