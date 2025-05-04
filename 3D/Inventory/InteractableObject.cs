using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string itemName; // Name of the item
    public bool isInInventory; // Flag for whether it's in the inventory
    private Inventory inventory;
    public Sprite itemIcon;

    // This function is called when the player interacts with the object.
    public virtual void Interact()
    {
        if(!isInInventory)
        {
            inventory = FindObjectOfType<Inventory>();
            // Add the object to the inventory.
            isInInventory = true;
            inventory.AddItem(this);
        }
        Destroy(gameObject);
    }

    // Function to outline the child object by changing its layer.
    public void SetOutline(bool enable)
    {
        // Assuming you have a child with an outline.
        Transform child = transform.GetChild(0); // Get first child (assuming this is the one with the outline)
        if (child != null)
        {
            if (enable)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Outline");
            }
            else
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }
}
