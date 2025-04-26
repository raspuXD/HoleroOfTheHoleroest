using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;  // Initial health of the enemy
    public GameObject theCoinPrefab;
    public Transform spawnPoint1, spawnPoint2, spawnPoint3;

    public void TakeDamage(int Damage)
    {
        health -= Damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        float randomChance = Random.Range(0f, 1f); // Generate a random number between 0 and 1

        if (randomChance <= 0.5f) // 50% chance
        {
            Instantiate(theCoinPrefab, spawnPoint1.position, Quaternion.identity);
        }
        else if (randomChance <= 0.75f) // 25% chance
        {
            Instantiate(theCoinPrefab, spawnPoint2.position, Quaternion.identity);
        }
        else if (randomChance <= 0.85f) // 10% chance
        {
            Instantiate(theCoinPrefab, spawnPoint3.position, Quaternion.identity);
        }
        // No coin spawn for remaining 15%

        Destroy(gameObject);
    }
}
