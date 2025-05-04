using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For UI elements like Image

public class Inventory : MonoBehaviour
{
    private List<InteractableObject> items = new List<InteractableObject>(); // List of items in the inventory
    public GameObject inventoryUI; // Reference to the Inventory UI GameObject
    public KeyCode OpenInventoryKey = KeyCode.I; // Key to open the inventory (default 'I')
    private bool inventoryOpen = false; // Track whether the inventory is open or closed

    // Reference to the UI slots where the items will be displayed
    public Image[] InventorySlots;

    void Update()
    {
        // Toggle the inventory when the OpenInventoryKey is pressed
        if (Input.GetKeyDown(OpenInventoryKey))
        {
            ToggleInventory();
        }

        // Update inventory slots with item icons
        UpdateInventoryUI();
    }

    // Toggle the inventory visibility
    private void ToggleInventory()
    {
        inventoryOpen = !inventoryOpen; // Flip the inventory state
        if (inventoryUI != null)
        {
            inventoryUI.SetActive(inventoryOpen); // Show or hide the inventory UI
        }
    }

    // Add item to the inventory
    public void AddItem(InteractableObject item)
    {
        if (items.Count < InventorySlots.Length) // Check if there is space in the inventory
        {
            items.Add(item);
            Debug.Log($"{item.itemName} added to inventory.");
        }
        else
        {
            Debug.LogWarning("Inventory is full!");
        }
    }

    // Show inventory (for debugging purposes)
    public void ShowInventory()
    {
        // Print out the list of items in the inventory
        foreach (var item in items)
        {
            Debug.Log("Inventory item: " + item.itemName);
        }
    }

    // Update the inventory UI slots with item icons
    private void UpdateInventoryUI()
    {
        // Loop through each slot in the inventory
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (i < items.Count) // If there is an item for this slot
            {
                InventorySlots[i].sprite = items[i].itemIcon; // Set the sprite of the slot
                InventorySlots[i].gameObject.SetActive(true); // Make the slot visible
            }
            else
            {
                InventorySlots[i].gameObject.SetActive(false); // Hide the slot if no item is in it
            }
        }
    }
}
