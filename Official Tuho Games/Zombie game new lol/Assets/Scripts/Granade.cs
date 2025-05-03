using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float explosionMinRadius = 2f;
    public float explosionMaxRadius = 5f;
    public float maxDamage = 100f;
    public float explosionDelay = 3f;

    public GameObject explosionEffect;

    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }

    IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(explosionDelay);
        Explode();
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionMaxRadius);

        foreach (Collider nearbyObject in colliders)
        {
            ZombieHealth zombieHealth = nearbyObject.GetComponent<ZombieHealth>();
            if (zombieHealth != null)
            {
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damage = CalculateDamage(distance);
                zombieHealth.TakeDamage(damage);
            }

            PlayerHealth playerHealth = nearbyObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);
                float damage = CalculateDamage(distance);
                playerHealth.TakeDamage(damage / 2);
            }
        }

        Destroy(gameObject);
    }

    float CalculateDamage(float distance)
    {
        if (distance <= explosionMinRadius)
        {
            // Maximum damage within the minimum radius
            return maxDamage;
        }
        else if (distance > explosionMinRadius && distance <= explosionMaxRadius)
        {
            // Damage falls off linearly from maxDamage to 0
            float distanceFromMin = distance - explosionMinRadius;
            float maxDistanceRange = explosionMaxRadius - explosionMinRadius;
            float damageFallOff = maxDamage * (1 - (distanceFromMin / maxDistanceRange));
            return Mathf.Clamp(damageFallOff, 0, maxDamage);
        }
        else
        {
            // Outside the maximum radius, no damage
            return 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionMaxRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, explosionMinRadius);
    }
}
