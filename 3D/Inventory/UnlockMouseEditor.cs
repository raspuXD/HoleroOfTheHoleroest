using UnityEngine;

public class UnlockMouseEditor : MonoBehaviour
{
    private KeyCode unlockKey = KeyCode.F9; // The key to toggle mouse unlock
    private bool mouseUnlocked = false; // Track if the mouse is unlocked

    // Update is called once per frame during play mode
    void Update()
    {
        // Check for the key press
        if (Input.GetKeyDown(unlockKey))
        {
            ToggleMouseLock();
        }
    }

    // Toggle mouse lock/unlock state
    private void ToggleMouseLock()
    {
        if (!mouseUnlocked)
        {
            // Unlock the mouse and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            mouseUnlocked = true;
            Debug.Log("Mouse Unlocked!");
        }
        else
        {
            // Lock the mouse and hide the cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            mouseUnlocked = false;
            Debug.Log("Mouse Locked!");
        }
    }
}
