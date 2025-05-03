using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour
{
    public int currentRound = 0;
    public TMP_Text roundText;

    public float timeBetweenStart = 6f;

    public int initialZombiesInFirstRound = 8;
    public int zombiesToSpawnAtThisRound;
    public int zombiesLeftToSpawn;

    [SerializeField] private GroundSpawner[] allSpawners;
    public Coroutine startTheRoundCoroutine;

    public List<ZombieHealth> theZombies = new List<ZombieHealth>();

    [SerializeField] GranadeManager granadeManager;
    bool isChangingRound = true;

    public List<WindowRepair> windows = new List<WindowRepair>();

    private void Start()
    {
        StartCoroutine(WaitForChange());
    }

    private void Update()
    {
        if(zombiesLeftToSpawn <= 0 && theZombies.Count == 0 && !isChangingRound)
        {
            StartCoroutine(WaitForChange());
        }
    }

    IEnumerator WaitForChange()
    {
        isChangingRound = true;
        yield return new WaitForSeconds(5f);
        ChangeTheRound();
    }


    public void ChangeTheRound()
    {
        currentRound++;
        HandleZombieRising();
        granadeManager.IncreaseGrenades(2);

        if(currentRound <= 999)
        {
            StartCoroutine(TypeText(currentRound.ToString(), 0.3f));
        }
        else
        {
            StartCoroutine(TypeText("999+", 0.3f));
        }

        if (startTheRoundCoroutine != null)
        {
            StopCoroutine(startTheRoundCoroutine);
        }

        startTheRoundCoroutine = StartCoroutine(NewRound());
    }

    private void TextSizeCheck()
    {
        if(roundText.text.Length == 1)
        {
            roundText.fontSize = 175;
        }
        else if(roundText.text.Length == 2)
        {
            roundText.fontSize = 160;
        }
        else if (roundText.text.Length == 3)
        {
            roundText.fontSize = 125;
        }
        else
        {
            roundText.fontSize = 106;
        }
    }

    private IEnumerator NewRound()
    {
        yield return new WaitForSeconds(timeBetweenStart);

        foreach (WindowRepair window in windows)
        {
            window.howMuchHasGiven = 0;
        }

        isChangingRound = false;

        while (zombiesLeftToSpawn > 0)
        {
            bool anySpawnerSpawning = false;

            foreach (GroundSpawner spawner in allSpawners)
            {
                if (spawner.isSpawningZombieCurrently == false && theZombies.Count < 24)
                {
                    spawner.StartTheSpawning();
                    zombiesLeftToSpawn--;
                    anySpawnerSpawning = true;

                    if (zombiesLeftToSpawn <= 0)
                    {
                        break;
                    }
                }
            }

            if (anySpawnerSpawning)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
    }

    void HandleZombieRising()
    {
        if (currentRound == 1)
        {
            zombiesToSpawnAtThisRound = initialZombiesInFirstRound;
        }
        else if (currentRound != 1 && currentRound < 5)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 4;
        }
        else if (currentRound >= 5 && currentRound <= 8)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 5;
        }
        else if (currentRound >= 8 && currentRound <= 13)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 7;
        }
        else if (currentRound >= 13 && currentRound <= 18)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 9;
        }
        else if (currentRound >= 18 && currentRound <= 25)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 11;
        }
        else if (currentRound >= 25)
        {
            zombiesToSpawnAtThisRound = zombiesToSpawnAtThisRound + 5;
        }

        zombiesLeftToSpawn = zombiesToSpawnAtThisRound;
    }

    // UI STUFF

    private IEnumerator TypeText(string text, float speed)
    {
        while (roundText.text.Length > 0)
        {
            roundText.text = roundText.text.Substring(0, roundText.text.Length - 1);
            yield return new WaitForSeconds(speed);
        }

        yield return new WaitForSeconds(.8f);

        foreach (char letter in text)
        {
            roundText.text += letter;
            TextSizeCheck();
            yield return new WaitForSeconds(speed);
        }
    }
}
