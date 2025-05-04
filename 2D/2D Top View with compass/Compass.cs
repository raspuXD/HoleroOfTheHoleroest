using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class CompassPoints
{
    public Transform theLocation;
    public float howBigCircleSoNoMetersShow; // radius for the circle area
}

public class Compass : MonoBehaviour
{
    public CompassPoints[] compassPoints;
    public RectTransform theArrowThatNeedsToBeRotated;
    public TMP_Text theDistanceToIt;

    void Update()
    {
        if (compassPoints.Length == 0) return;

        Transform nearestPoint = null;
        float shortestDistance = Mathf.Infinity;

        foreach (CompassPoints point in compassPoints)
        {
            Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 pointPosition = new Vector2(point.theLocation.position.x, point.theLocation.position.y);
            float distance = Vector2.Distance(currentPosition, pointPosition);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestPoint = point.theLocation;
            }
        }

        if (nearestPoint != null)
        {
            Vector2 direction = new Vector2(nearestPoint.position.x - transform.position.x, nearestPoint.position.y - transform.position.y);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            theArrowThatNeedsToBeRotated.rotation = Quaternion.Euler(0, 0, angle);

            // If inside the circle, don't show the distance text
            if (shortestDistance <= 10)
            {
                theDistanceToIt.text = "";  // No text if inside the circle
            }
            else
            {
                theDistanceToIt.text = $"{Mathf.RoundToInt(shortestDistance)} m";
            }
        }
    }

    // Draw Gizmos in the editor
    private void OnDrawGizmos()
    {
        if (compassPoints.Length == 0) return;

        Gizmos.color = Color.green;  // Choose a color for the circle

        foreach (CompassPoints point in compassPoints)
        {
            if (point.theLocation != null)
            {
                Gizmos.DrawWireSphere(point.theLocation.position, point.howBigCircleSoNoMetersShow);  // Draw the circle radius
            }
        }
    }
}
