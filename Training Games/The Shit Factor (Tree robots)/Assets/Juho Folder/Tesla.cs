using System.Collections;
using UnityEngine;

public class Tesla : MonoBehaviour
{
    public Transform shootPoint;
    public float detectionRadius = 5f;
    public int minPoints = 3;
    public int maxPoints = 6;
    public float offsetAmount = 0.5f;
    public float updateRate = 0.05f;
    public float shootDuration = 2f;
    public float pauseDuration = 4f;
    public float damageInterval = 0.4f;

    public LineRenderer lineRenderer;
    private float timer;
    private bool isShooting = false;

    private EnemyMovement enemyMovement;
    private float damageTimer = 0f;
    public Transform target;

    void Start()
    {
        lineRenderer.positionCount = 0;
        StartCoroutine(ShootAndPauseCycle());
    }

    void Update()
    {
        if (isShooting)
        {
            timer += Time.deltaTime;

            if (timer >= shootDuration)
            {
                isShooting = false;  // Stop shooting
                timer = 0f;
                ClearLineRenderer();  // Clear the line renderer when we stop shooting
            }
            else
            {
                damageTimer += Time.deltaTime;
                if (enemyMovement != null && damageTimer >= damageInterval)
                {
                    enemyMovement.TakeDamage(1);
                    damageTimer = 0f;
                }

                if (timer >= updateRate)
                {
                    DrawLightning();  // Draw the lightning every updateRate seconds
                    timer = 0f;
                }
            }
        }
        else if (target == null)  // Clear line renderer when no target
        {
            ClearLineRenderer();
        }
    }

    void DrawLightning()
    {
        if (shootPoint == null || target == null) return;

        int pointCount = Random.Range(minPoints, maxPoints + 2);
        lineRenderer.positionCount = pointCount;

        Vector2 start = shootPoint.position;
        Vector2 end = target.position;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(pointCount - 1, end);

        for (int i = 1; i < pointCount - 1; i++)
        {
            float t = (float)i / (pointCount - 1);
            Vector2 point = Vector2.Lerp(start, end, t);
            point += new Vector2(Random.Range(-offsetAmount, offsetAmount), Random.Range(-offsetAmount, offsetAmount));
            lineRenderer.SetPosition(i, point);
        }

        if (target == null)  // If no valid target is found, clear LineRenderer
        {
            ClearLineRenderer();
        }
    }

    void FindClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Enemy") || collider.GetComponent<EnemyMovement>() != null)
            {
                float distance = Vector2.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = collider.transform;
                    enemyMovement = collider.GetComponent<EnemyMovement>();
                }
            }
        }

        if (target == null)  // If no valid target is found, clear LineRenderer
        {
            ClearLineRenderer();
        }
    }

    IEnumerator ShootAndPauseCycle()
    {
        while (true)
        {
            FindClosestEnemy();  // Find the closest enemy

            if (target != null)
            {
                isShooting = true;
                timer = 0f;

                // Wait until the shooting phase is complete before pausing
                yield return new WaitForSeconds(shootDuration);
                isShooting = false;  // Stop shooting
                ClearLineRenderer();  // Clear the line renderer after shooting

                // After shooting, wait for the pause duration before starting again
                yield return new WaitForSeconds(pauseDuration);
            }
            else
            {
                // If no target, clear LineRenderer and wait for the pause duration
                ClearLineRenderer();
                yield return new WaitForSeconds(pauseDuration);
            }
        }
    }

    // Clears the LineRenderer to remove leftover lightning effects
    void ClearLineRenderer()
    {
        lineRenderer.positionCount = 0;  // Clear the line when no target is present
    }

    private void OnDrawGizmos()
    {
        if (transform != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
