using TMPro;
using UnityEngine;

public class HumanResource : MonoBehaviour
{
    public int howManyPeople = 100; // Total people available
    [SerializeField] TMP_Text humanAmountText;
    [SerializeField] TMP_Text cooldownText;
    [SerializeField] TMP_ColorGradient red, orange, white;

    private float cooldownTime = 15; // Cooldown time in seconds
    private float currentCooldownTime = 0f; // Current cooldown time tracking
    public EnemySpawner spawner;

    private void Start()
    {
        UpdateTheText();
    }

    private void Update()
    {
        // Decrease the cooldown timer
        if (currentCooldownTime > 0)
        {
            currentCooldownTime -= Time.deltaTime;
            cooldownText.text = currentCooldownTime.ToString("F0") + "s"; // Update the cooldown display
        }
        else
        {
            cooldownText.text = "Send SOS"; // Cooldown is over
        }

        // Listen for the Q key press if cooldown is over
        if (Input.GetKeyDown(KeyCode.Q) && currentCooldownTime <= 0)
        {
            TriggerHumanEvent();
            currentCooldownTime = cooldownTime; // Reset cooldown
        }
    }

    // Handle the human resource event triggered by Q key
    private void TriggerHumanEvent()
    {
        float chance = Random.Range(0f, 1f); // Random chance to decide what happens

        if (chance < 0.85f) // 75% chance to add people
        {
            int peopleToAdd = Random.Range(1, 11); // Add 1 to 10 people
            GetHumans(peopleToAdd);
            Debug.Log("Added " + peopleToAdd + " people.");
        }
        else // 25% chance to log a message
        {
            spawner.SpawnOnceLOL();
        }
    }

    // Use people based on the given amount (decreases the number of people)
    public bool UseHumans(int amount)
    {
        if (howManyPeople >= amount)
        {
            howManyPeople -= amount;
            Debug.Log("Using humans: " + amount);
            UpdateTheText();
            return true;
        }

        Debug.Log("Not enough humans to use.");
        return false;
    }

    // Add people (increases the number of people)
    public void GetHumans(int amount)
    {
        AudioManager.Instance.PlaySFX("YAY");

        howManyPeople += amount;
        UpdateTheText();
    }

    // Update the text based on how many people are left
    public void UpdateTheText()
    {
        humanAmountText.text = howManyPeople.ToString("F0") + " People";

        if (howManyPeople > 50)
        {
            humanAmountText.colorGradientPreset = white;
        }
        else if (howManyPeople < 15)
        {
            humanAmountText.colorGradientPreset = red;
        }
        else
        {
            humanAmountText.colorGradientPreset = orange;
        }
    }

    // Returns the percentage of people (1 person = 1%)
    public float GetHumanPercentage()
    {
        return Mathf.Clamp((float)howManyPeople, 0f, 100f);
    }
}
