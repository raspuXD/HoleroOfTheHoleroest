using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform target, rotateThis;
    public float rotationSpeed = 5f;

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 2f;
    float fireCooldown;

    public float detectionRange = 10f; // The range to detect enemies

    void Update()
    {
        FindClosestVisibleEnemy();

        float targetAngle = 0f;

        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = fireRate;
            }
        }
        else
        {
            targetAngle = -180f;
        }

        float currentAngle = rotateThis.eulerAngles.z;
        float smoothAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * rotationSpeed);
        rotateThis.localRotation = Quaternion.Euler(0f, 0f, smoothAngle);

        fireCooldown -= Time.deltaTime;
    }

    void FindClosestVisibleEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRange);

        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D collider in enemiesInRange)
        {
            if (collider.CompareTag("Enemy"))
            {
                float dist = Vector2.Distance(transform.position, collider.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = collider.transform;
                }
            }
        }

        target = closest;
    }

    void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            AudioManager.Instance.PlaySFX("Cannon");
        }
    }

    // Draw a gizmo to visualize the detection range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set the color of the gizmo
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Draw a wire sphere at the transform's position
    }
}
