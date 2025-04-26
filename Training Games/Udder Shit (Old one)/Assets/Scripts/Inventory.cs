using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // To work with UI components
using TMPro;  // To work with TextMeshPro components

public class Inventory : MonoBehaviour
{
    // Inventory values
    public float Insanity = 0f;  // Starts at 0
    public int Milk = 5;  // Initial milk amount
    public int Bullets = 10;  // Initial bullet count

    public Color darkRed;
    // UI components
    public Image insanityFillImage;  // Image component to represent insanity as a fill (instead of Slider)

    // TextMeshPro components to display Milk and Bullets
    public TMP_Text milkText;  // TextMeshPro reference for milk amount
    public TMP_Text bulletsText;  // TextMeshPro reference for bullet count

    // Insanity dynamics
    public float InsanityIncreaseRate = 1f;  // Rate of insanity increase per second
    public float InsanityDecreaseRate = 0.5f;  // Rate of insanity decrease
    SceneHandler sceneHandler;


    private void Start()
    {
        sceneHandler = FindObjectOfType<SceneHandler>();
    }

    // Called once per frame
    void Update()
    {
        // Example condition to increase insanity (could be based on events in the game)
        if (Insanity < 100f)  // Make sure insanity does not exceed 100
        {
            Insanity += InsanityIncreaseRate * Time.deltaTime;
        }
        else
        {
            sceneHandler.LoadSceneNamed("gameOver");
        }

        // Update the insanity fill image's fill amount based on the current Insanity value
        if (insanityFillImage != null)
        {
            insanityFillImage.fillAmount = Insanity / 100f;  // Normalize Insanity to a 0-1 range
        }

        // Optional: Customize the fill color of the insanity bar (e.g., make it red as insanity increases)
        if (insanityFillImage != null)
        {
            float insanityPercentage = Insanity / 100f;  // Normalize Insanity to a 0-1 range
            insanityFillImage.color = Color.Lerp(Color.red, darkRed, insanityPercentage);  // Lerp color from green to red
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Milk > 0)
            {
                DecreaseMilk(1);  // Drink one milk
                DecreaseInsanity(10f);  // Reduce insanity by 10
            }
            else
            {
                Debug.Log("No milk to drink!");
            }
        }
        // Update the Milk and Bullets text displays
        UpdateInventoryText();
    }

    // Method to update the Milk and Bullets text displays
    private void UpdateInventoryText()
    {
        if (milkText != null)
        {
            milkText.text = Milk.ToString();  // Update the milk amount text
        }

        if (bulletsText != null)
        {
            bulletsText.text = Bullets.ToString();  // Update the bullets amount text
        }
    }

    // Method to increase milk
    public void IncreaseMilk(int amount)
    {
        Milk += amount;
        Debug.Log("Milk increased! Current milk: " + Milk);
    }

    // Method to decrease milk (e.g., for actions like using milk as a resource)
    public void DecreaseMilk(int amount)
    {
        if (Milk >= amount)
        {
            Milk -= amount;
            Debug.Log("Milk decreased! Current milk: " + Milk);
        }
        else
        {
            Debug.Log("Not enough milk!");
        }
    }

    // Method to increase bullets
    public void IncreaseBullets(int amount)
    {
        Bullets += amount;
        Debug.Log("Bullets increased! Current bullets: " + Bullets);
    }

    // Method to decrease bullets (e.g., for shooting)
    public void DecreaseBullets(int amount)
    {
        if (Bullets >= amount)
        {
            Bullets -= amount;
            Debug.Log("Bullets decreased! Current bullets: " + Bullets);
        }
        else
        {
            Debug.Log("Not enough bullets!");
        }
    }

    // Method to handle insanity increase (for when specific triggers happen in the game)
    public void IncreaseInsanity(float amount)
    {
        Insanity = Mathf.Min(Insanity + amount, 100f);  // Clamp the insanity value to max of 100
        Debug.Log("Insanity increased! Current insanity: " + Insanity);
    }

    // Method to handle insanity decrease (for when calming actions or successful defense happens)
    public void DecreaseInsanity(float amount)
    {
        Insanity = Mathf.Max(Insanity - amount, 0f);  // Ensure insanity doesn't go below 0
        Debug.Log("Insanity decreased! Current insanity: " + Insanity);
    }

    // You could also add a method to display values for debugging purposes
    public void DisplayInventoryStatus()
    {
        Debug.Log($"Insanity: {Insanity}, Milk: {Milk}, Bullets: {Bullets}");
    }
}
