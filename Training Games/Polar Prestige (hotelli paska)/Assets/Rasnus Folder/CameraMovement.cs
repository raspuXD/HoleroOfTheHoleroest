using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Speed of the camera movement

    [Header("Movement Boundaries")]
    public float minX = -10f; // Minimum X position
    public float maxX = 10f;  // Maximum X position
    public float minY = -5f;  // Minimum Y position
    public float maxY = 5f;   // Maximum Y position
    public TutorialPopUp howToBuy;
    int hasSeenPop = 0;

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Get input for horizontal and vertical movement
        float moveX = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
        float moveY = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

        // Calculate new position
        Vector3 newPosition = transform.position + new Vector3(moveX, moveY, 0) * moveSpeed * Time.deltaTime;

        // Restrict movement within boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Apply new position to the camera
        transform.position = newPosition;


        if ((transform.position.x <= -11f || transform.position.x >= 12f) && hasSeenPop == 0)
        {
            howToBuy.StartThePopUp("You can buy rooms or upgrade the furniture inside them to earn more money");
            hasSeenPop = 1;
        }
    }
}
