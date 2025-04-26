using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // To work with UI components
using TMPro;  // To work with TextMeshPro components

public class Inventory : MonoBehaviour
{
    public int Milk = 5;  // Initial milk amount
    public int Bullets = 10;  // Initial bullet count
    public int Money = 0;

    public TMP_Text milkText;  // TextMeshPro reference for milk amount
    public TMP_Text bulletsText;
    public TMP_Text moneyText; // TextMeshPro reference for bullet count

    public InventoryShow show;

    public PlayerHealth playerHealth; // Reference to PlayerHealth script

    // Called once per frame
    void Update()
    {
        // Check if the player presses the 'R' key to drink milk
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Milk > 0) // Check if the player has milk
            {
                Milk--; // Decrease milk count by 1
                playerHealth.RestoreHealth(40); // Restore health by 20 (this can be customized)
                AudioManager.Instance.PlaySFX("DRANK");
                Debug.Log("Drank milk. Health restored!");
            }
            else
            {
                Debug.Log("No milk to drink!");
            }
        }

        // Update the inventory display (milk, bullets, money)
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

        if (moneyText != null)
        {
            moneyText.text = Money.ToString() + "€";  // Update the money text
        }
    }

    public void IncreaseMoney(int amount)
    {
        Money += amount;
        AudioManager.Instance.PlaySFX("MomeGe");
        show.ActivateThis();
        show.HideInventory();
        Debug.Log("Money increased! Current money: " + Money);
    }

    public void DecreaseMoney(int amount)
    {
        Money -= amount;
        show.ActivateThis();
        show.HideInventory();
    }

    public void IncreaseMilk(int amount)
    {
        Milk += amount;
        AudioManager.Instance.PlaySFX("MilkGet");
        show.ActivateThis();
        show.HideInventory();
        Debug.Log("Milk increased! Current milk: " + Milk);
    }

    // Method to decrease milk (e.g., for actions like using milk as a resource)
    public void DecreaseMilk(int amount)
    {
        if (Milk >= amount)
        {
            Milk -= amount;
            show.ActivateThis();
            show.HideInventory();
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
        AudioManager.Instance.PlaySFX("BulletsGet");
        show.ActivateThis();
        show.HideInventory();
        Debug.Log("Bullets increased! Current bullets: " + Bullets);
    }

    // Method to decrease bullets (e.g., for shooting)
    public void DecreaseBullets(int amount)
    {
        if (Bullets >= amount)
        {
            Bullets -= amount;
            show.ActivateThis();
            show.HideInventory();
            Debug.Log("Bullets decreased! Current bullets: " + Bullets);
        }
        else
        {
            Debug.Log("Not enough bullets!");
        }
    }
}
