using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{

    public enum ItemType
    {
        Milk,
        Money,
        Bullets
    }

    public ItemType itemType;  // Specifies the type of item this pickup represents
    public int amount = 1;      // The amount of items to add when picked up

    private Inventory inventory;  // Reference to the player's inventory
    private PointSystem points;
    void Start()
    {
        // Get the inventory script from the player (assuming player object has Inventory script attached)
        inventory = GameObject.FindObjectOfType<Inventory>();
        points = FindObjectOfType<PointSystem>();
    }

    // Trigger event when the player enters the pickup area
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))  // Check if the object entering is the player
        {
            // Add item to inventory based on the type
            switch (itemType)
            {
                case ItemType.Milk:
                    inventory.IncreaseMilk(amount);
                    break;

                case ItemType.Money:
                    // You can implement money handling similarly if you have a money system
                    points.points++;
                    break;

                case ItemType.Bullets:
                    inventory.IncreaseBullets(amount);
                    break;
            }

            // Optionally, destroy the item after pickup
            Destroy(gameObject);  // Remove the item from the scene
        }
    }
}
