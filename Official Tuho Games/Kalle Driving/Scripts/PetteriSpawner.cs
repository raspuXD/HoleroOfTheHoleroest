using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetteriSpawner : MonoBehaviour
{
    public Transform[] walkPoints;
    public Petteri thePetteriToSpawn;
    public float howOften = 7f;
    public Transform spawnPoint;
    public Color gizmoColor;
    private bool initialSpawningDone = false;
    public bool canSpawn = true;

    private List<Petteri> spawnedPetteris = new List<Petteri>(); // List to track spawned Petteris

    private void Start()
    {
        StartCoroutine(InitialSpawn());
    }

    private IEnumerator InitialSpawn()
    {
        // Spawn Petteri at every 3rd walk point
        for (int i = 0; i < walkPoints.Length; i += 3)
        {
            if (thePetteriToSpawn != null && walkPoints[i] != null)
            {
                Petteri newPetteri = Instantiate(thePetteriToSpawn, walkPoints[i].position, Quaternion.identity);
                newPetteri.theWalkingPoints = walkPoints;
                newPetteri.SetStartIndex(i); // Ensure it starts from the next walk point
                spawnedPetteris.Add(newPetteri); // Add to list of spawned Petteris
            }
            yield return new WaitForSeconds(.1f); // Slight delay between spawns
        }

        initialSpawningDone = true;
        StartCoroutine(SpawnPetteriAtSpawnPoint());
    }

    private IEnumerator SpawnPetteriAtSpawnPoint()
    {
        while (canSpawn)
        {
            yield return new WaitForSeconds(howOften);

            if (initialSpawningDone && thePetteriToSpawn != null)
            {
                Petteri newPetteri = Instantiate(thePetteriToSpawn, spawnPoint.position, Quaternion.identity);
                newPetteri.theWalkingPoints = walkPoints;
                newPetteri.SetStartIndex(0); // Start from the first waypoint
                spawnedPetteris.Add(newPetteri); // Add to list of spawned Petteris
            }
        }
    }

    // Method to destroy all spawned Petteris
    public void DestroyAllPetteris()
    {
        foreach (Petteri petteri in spawnedPetteris)
        {
            if (petteri != null)
            {
                Destroy(petteri.gameObject); // Destroy the Petteri object
            }
        }

        spawnedPetteris.Clear(); // Clear the list after destroying
    }

    // Method to stop spawning new Petteris
    public void StopSpawning()
    {
        canSpawn = false; // Prevent new Petteris from being spawned
        StopAllCoroutines(); // Stop the spawning coroutines
    }

    private void OnDrawGizmos()
    {
        if (walkPoints == null || walkPoints.Length == 0) return;

        Gizmos.color = gizmoColor;
        for (int i = 0; i < walkPoints.Length - 1; i++)
        {
            if (walkPoints[i] != null && walkPoints[i + 1] != null)
            {
                Gizmos.DrawLine(walkPoints[i].position, walkPoints[i + 1].position);
                Gizmos.DrawSphere(walkPoints[i].position, 0.2f);
            }
        }

        // Draw last point sphere
        if (walkPoints[walkPoints.Length - 1] != null)
        {
            Gizmos.DrawSphere(walkPoints[walkPoints.Length - 1].position, 0.2f);
        }
    }
}
