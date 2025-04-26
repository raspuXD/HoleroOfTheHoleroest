using UnityEngine;
using UnityEngine.AI;

public class Snake : MonoBehaviour
{
    public int damageAmount = 20;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            AudioManager.Instance.PlaySFX("SNOOK");
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
