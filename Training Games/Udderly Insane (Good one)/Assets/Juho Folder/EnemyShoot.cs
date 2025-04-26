using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public Transform firePoint;
    public GameObject BulletPrefab;
    public float bulletSpeed = 10f;

    public Transform target; // Target that the enemy will shoot at
    public RotateEnemy rotation;
    public float shootingRange = 5f; // The maximum distance to shoot

    private void Start()
    {
        if (firePoint == null || BulletPrefab == null || rotation == null)
        {
            Debug.LogError("Missing required references in EnemyShoot script.");
            return;
        }

        StartCoroutine(Shoot());
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            if (target != null && Vector2.Distance(transform.position, target.position) <= shootingRange)
            {
                yield return new WaitForSeconds(2f);

                if (rotation != null)
                {
                    rotation.canRotate = false;
                    rotation.RotateDifferent();

                    yield return new WaitForSeconds(.5f);

                    GameObject bullet = Instantiate(BulletPrefab, firePoint.position, firePoint.rotation);
                    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = firePoint.up * bulletSpeed; // Shoot the bullet in the firePoint's up direction
                    }
                    else
                    {
                        Debug.LogWarning("Bullet prefab does not have a Rigidbody2D component!");
                    }

                    yield return new WaitForSeconds(.5f);

                    rotation.canRotate = true;
                }
                else
                {
                    Debug.LogWarning("Rotation script is missing.");
                }
            }
            yield return null;
        }
    }
}
