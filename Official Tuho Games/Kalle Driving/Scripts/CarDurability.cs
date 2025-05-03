using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarDurability : MonoBehaviour
{
    public float currentDura;
    public float maxDura;
    public Image duraBar; // Reference to the UI Image
    bool hasBeenDone;
    [SerializeField] GameObject[] toDeactivate;
    [SerializeField] GameObject[] toActivate;
    public SceneHandler sceneHandler;

    public PetteriSpawner[] spawnerti;
    Petteri[] allpetterit;

    public TMP_Text theText;

    public Animator animator;

    public Car car;

    private void Start()
    {
        // Apply the upgrade multiplier to durability
        int multiplier = PlayerPrefs.GetInt("DuraUpgrade", 1);

        // Update maxDura based on the level pattern
        if (multiplier == 1)
        {
            maxDura = 10;  // Level 1 -> 10
        }
        else if (multiplier == 2)
        {
            maxDura = 20;  // Level 2 -> 20
        }
        else if (multiplier == 3)
        {
            maxDura = 30;  // Level 3 -> 30
        }
        else
        {
            maxDura = 30 + (multiplier - 3) * 5;  // Levels after 3 -> increase by 5
        }

        // Set current durability to max at the start
        currentDura = maxDura;
    }

    private void Update()
    {
        // Only perform the actions when the durability reaches 0 and hasn't been done yet
        if (currentDura <= 0 && !hasBeenDone)
        {
            hasBeenDone = true;
            foreach (PetteriSpawner spawner in spawnerti)
            {
                spawner.StopSpawning();
                spawner.DestroyAllPetteris();
                Destroy(spawner.gameObject);
            }

            StartCoroutine(GameOverWait());
        }

        // Continuously update durability UI
        UpdateDurabilityUI();
    }

    IEnumerator GameOverWait()
    {
        car.isntDoingAnimation = false;
        animator.enabled = true;
        yield return new WaitForSeconds(2.6f);

        foreach (GameObject obj in toDeactivate)
        {
            obj.SetActive(false);
        }

        // Activate the specified game objects
        foreach (GameObject obj in toActivate)
        {
            obj.SetActive(true);
        }

        sceneHandler.UpdateMouseState(true);
    }


    private void UpdateDurabilityUI()
    {
        // Ensure the durability bar updates only when it's not null

        if(currentDura <= maxDura)
        {
            theText.text = "Auton kunto";
        }
        else
        {
            float howMuchOver = currentDura - maxDura;
            theText.text = "Auton kunto  <size=36><color=green>+" + howMuchOver.ToString("F0") + " kiitos Petterin";
        }

        if (duraBar != null)
        {
            duraBar.fillAmount = Mathf.Clamp01(currentDura / maxDura);
        }
    }
}
