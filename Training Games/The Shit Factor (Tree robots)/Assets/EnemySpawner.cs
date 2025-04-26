using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemy1, enemy2;
    public int amountOfEnemy1, amountOfEnemy2;
}

public class EnemySpawner : MonoBehaviour
{
    public float timeBetweenEachSpawn = 0.5f;
    public float timeAfterWaveClear = 3f;
    public Wave[] waves;
    public Transform[] spawnPoints;

    private int currentWaveIndex = 0;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void StartPlaying()
    {
        StartCoroutine(SpawnWaves());
        AudioManager.Instance.ChangeMusic("Action", .24f, .24f);
    }

    public void SpawnOnceLOL()
    {
        AudioManager.Instance.PlaySFX("attack");
        Instantiate(waves[0].enemy1, spawnPoints[0].position, Quaternion.identity);
        Instantiate(waves[0].enemy1, spawnPoints[3].position, Quaternion.identity);
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];

            List<GameObject> enemiesToSpawn = new List<GameObject>();

            for (int i = 0; i < currentWave.amountOfEnemy1; i++)
                enemiesToSpawn.Add(currentWave.enemy1);

            for (int i = 0; i < currentWave.amountOfEnemy2; i++)
                enemiesToSpawn.Add(currentWave.enemy2);

            ShuffleList(enemiesToSpawn);
            AudioManager.Instance.PlaySFX("attack");

            foreach (GameObject enemyPrefab in enemiesToSpawn)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                GameObject spawned = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
                spawnedEnemies.Add(spawned);
                yield return new WaitForSeconds(timeBetweenEachSpawn);
            }

            bool isLastWave = currentWaveIndex == waves.Length - 1;
            yield return StartCoroutine(WaitUntilAllEnemiesAreDead());

            if (isLastWave)
            {
                SceneHandler scene = FindObjectOfType<SceneHandler>();
                scene.LoadSceneNamed("win");
            }

            currentWaveIndex++;
        }
    }

    private IEnumerator WaitUntilAllEnemiesAreDead()
    {
        while (true)
        {
            // Clean up null entries (destroyed enemies)
            spawnedEnemies.RemoveAll(e => e == null);

            if (spawnedEnemies.Count == 0)
                break;

            yield return null;
        }

        yield return new WaitForSeconds(timeAfterWaveClear);
    }

    private void ShuffleList(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
