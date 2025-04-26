
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera playerCamera; // The camera used for raycasting
    [SerializeField] private float rayDistance = 5f; // Max distance for raycasting
    [SerializeField] private KeyCode interactionKey = KeyCode.E; // Key to interact

    [Header("Target Position")]
    [SerializeField] private Transform targetPosition; // Target position to hold the object
    [SerializeField] private LayerMask pickupLayerMask; // LayerMask for raycast when picking up (ignore Player)
    private GameObject heldObject; // Reference to the currently held object


    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            if (heldObject == null)
            {
                HandlePickup();
            }
            else
            {
                HandleDrop();
            }
        }
    }

    private void HandlePickup()
    {
        // Check if the target position is already holding an object
        if (targetPosition.childCount > 0)
        {
            Debug.Log("Target position already has an object.");
            return;
        }

        // Perform a raycast from the center of the camera for pickup
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, pickupLayerMask))
        {
            // Check if the object has the InteractableObject component
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();

            if (interactable != null)
            {
                // Pick up the object
                heldObject = interactable.gameObject;

                // Move the object to the target position and make it a child of the target position
                interactable.MoveToTarget(targetPosition);
                Debug.Log($"Picked up {heldObject.name}.");
            }
            else
            {
                Debug.Log("Hit object is not interactable.");
            }
        }
        else
        {
            Debug.Log("No object detected within range.");
        }
    }

    private void HandleDrop()
    {


            heldObject.transform.SetParent(null);

            // Release the object and make its Rigidbody non-kinematic
            InteractableObject interactable = heldObject.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                interactable.ReleaseObject();
            }


            // Clear the held object reference
            heldObject = null;

    }
}