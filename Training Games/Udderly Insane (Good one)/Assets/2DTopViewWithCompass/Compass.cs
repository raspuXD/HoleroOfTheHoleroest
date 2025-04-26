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
    public RectTransform theArrowThatNeedsToBeRotatedTowardCowEnemy;
    public TMP_Text theDistanceToIt;
    private Coroutine cowCheckCoroutine;

    void Start()
    {
        cowCheckCoroutine = StartCoroutine(CheckForCows());
    }

    void Update()
    {
        if (compassPoints.Length > 0)
        {
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
                theDistanceToIt.text = shortestDistance <= 3 ? "" : $"Kai is at {Mathf.RoundToInt(shortestDistance)} m";
            }
        }
    }

    IEnumerator CheckForCows()
    {
        while (true)
        {
            GameObject[] cowEnemies = GameObject.FindGameObjectsWithTag("EnemyCow");
            Transform nearestCow = null;
            float shortestCowDistance = Mathf.Infinity;

            foreach (GameObject cow in cowEnemies)
            {
                float distance = Vector2.Distance(transform.position, cow.transform.position);
                if (distance < shortestCowDistance)
                {
                    shortestCowDistance = distance;
                    nearestCow = cow.transform;
                }
            }

            if (nearestCow != null)
            {
                Vector2 cowDirection = new Vector2(nearestCow.position.x - transform.position.x, nearestCow.position.y - transform.position.y);
                float cowAngle = Mathf.Atan2(cowDirection.y, cowDirection.x) * Mathf.Rad2Deg;
                theArrowThatNeedsToBeRotatedTowardCowEnemy.rotation = Quaternion.Euler(0, 0, cowAngle);
                theArrowThatNeedsToBeRotatedTowardCowEnemy.gameObject.SetActive(true);
            }
            else
            {
                theArrowThatNeedsToBeRotatedTowardCowEnemy.gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1f);
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
