using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance = 3f; // Distance at which player can interact with objects
    public KeyCode interactKey; // The key for interacting
    private InteractableObject currentInteractable = null;

    private void Update()
    {
        CheckForInteractable();

        // If an interactable object is in range and the player presses the interact key.
        if (currentInteractable != null && Input.GetKeyDown(interactKey))
        {
            currentInteractable.Interact(); // Interact with the object (add to inventory)
            currentInteractable.SetOutline(false); // Disable outline after interaction
            currentInteractable = null; // Reset the current interactable object
        }
    }

    private void CheckForInteractable()
    {
        // Cast a ray forward from the player's position to check for interactables
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                if (currentInteractable != interactable) 
                {
                    // If the interactable object changes, turn off the previous object's outline
                    if (currentInteractable != null)
                    {
                        currentInteractable.SetOutline(false);
                    }

                    currentInteractable = interactable;
                    interactable.SetOutline(true); // Enable outline for the new interactable
                }
            }
        }
        else
        {
            if (currentInteractable != null)
            {
                currentInteractable.SetOutline(false); // Disable outline if nothing is in range
                currentInteractable = null;
            }
        }
    }
}
