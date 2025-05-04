using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraPositionsThatTriggerSomething
{
    public bool needsX, needsY;
    public float whereToBeX, whereToBeY;
    public bool hasBeenTriggered = false;
    public float triggerThreshold = 0.1f;
}

public class TwoD_Camera : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Movement Boundaries")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Movement Settings")]
    public bool usesWASD;
    public bool usesMouse;
    public float mouseEdgeThreshold = 100f;

    [Header("Events on certain locations")]
    public CameraPositionsThatTriggerSomething[] triggerAreas;

    [Header("Cursor Settings")]
    public Texture2D cursorUp;
    public Texture2D cursorDown;
    public Texture2D cursorLeft;
    public Texture2D cursorRight;
    public Texture2D cursorUpLeft;
    public Texture2D cursorUpRight;
    public Texture2D cursorDownLeft;
    public Texture2D cursorDownRight;
    public Texture2D cursorDefault;

    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }

        HandleMovement();
        CheckTriggers();
        UpdateCursor();
    }

    private void HandleMovement()
    {
        Vector3 newPosition = transform.position;

        if (usesWASD)
        {
            float moveX = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            float moveY = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
            newPosition += new Vector3(moveX, moveY, 0).normalized * moveSpeed * Time.deltaTime;
        }

        if (usesMouse)
        {
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector3 direction = Vector3.zero;

            if (mouseScreenPos.x <= mouseEdgeThreshold)
                direction.x = -1;
            else if (mouseScreenPos.x >= Screen.width - mouseEdgeThreshold)
                direction.x = 1;

            if (mouseScreenPos.y <= mouseEdgeThreshold)
                direction.y = -1;
            else if (mouseScreenPos.y >= Screen.height - mouseEdgeThreshold)
                direction.y = 1;

            if (direction != Vector3.zero)
                newPosition += direction.normalized * moveSpeed * Time.deltaTime;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    private void CheckTriggers()
    {
        foreach (var area in triggerAreas)
        {
            if (!area.hasBeenTriggered)
            {
                bool isWithinX = !area.needsX || Mathf.Abs(transform.position.x - area.whereToBeX) <= area.triggerThreshold;
                bool isWithinY = !area.needsY || Mathf.Abs(transform.position.y - area.whereToBeY) <= area.triggerThreshold;

                if (isWithinX && isWithinY)
                {
                    Debug.Log($"Trigger activated at: X = {area.whereToBeX}, Y = {area.whereToBeY}");
                    area.hasBeenTriggered = true;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(maxX, minY, 0));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(maxX, maxY, 0));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(minX, maxY, 0));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(minX, minY, 0));
    }

    private void UpdateCursor()
    {
        if (usesMouse)
        {
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector3 direction = Vector3.zero;

            if (mouseScreenPos.x <= mouseEdgeThreshold)
                direction.x = -1;
            else if (mouseScreenPos.x >= Screen.width - mouseEdgeThreshold)
                direction.x = 1;

            if (mouseScreenPos.y <= mouseEdgeThreshold)
                direction.y = -1;
            else if (mouseScreenPos.y >= Screen.height - mouseEdgeThreshold)
                direction.y = 1;

            // Update the cursor based on direction
            if (direction.x == 0 && direction.y == 1)
                Cursor.SetCursor(cursorUp, Vector2.zero, CursorMode.Auto); // Up
            else if (direction.x == 0 && direction.y == -1)
                Cursor.SetCursor(cursorDown, Vector2.zero, CursorMode.Auto); // Down
            else if (direction.x == -1 && direction.y == 0)
                Cursor.SetCursor(cursorLeft, Vector2.zero, CursorMode.Auto); // Left
            else if (direction.x == 1 && direction.y == 0)
                Cursor.SetCursor(cursorRight, Vector2.zero, CursorMode.Auto); // Right
            else if (direction.x == -1 && direction.y == 1)
                Cursor.SetCursor(cursorUpLeft, Vector2.zero, CursorMode.Auto); // Up-Left
            else if (direction.x == 1 && direction.y == 1)
                Cursor.SetCursor(cursorUpRight, Vector2.zero, CursorMode.Auto); // Up-Right
            else if (direction.x == -1 && direction.y == -1)
                Cursor.SetCursor(cursorDownLeft, Vector2.zero, CursorMode.Auto); // Down-Left
            else if (direction.x == 1 && direction.y == -1)
                Cursor.SetCursor(cursorDownRight, Vector2.zero, CursorMode.Auto); // Down-Right
            else
                Cursor.SetCursor(cursorDefault, Vector2.zero, CursorMode.Auto); // Default
        }
    }
}
