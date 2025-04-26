using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enemy_Health : MonoBehaviour
{
    public int health = 100;  // Initial health of the enemy
    int maxHealth;
    public GameObject theCoinPrefab;
    public Transform spawnPoint1, spawnPoint2, spawnPoint3;

    [SerializeField] private SpriteRenderer spriteRenderer; // Assign in Inspector
    [SerializeField] private float hitEffectDuration = 0.5f; // Effect time
    [SerializeField] private float maxHitEffect = 1.0f; // Max blend value

    private Material material;
    private Coroutine hitEffectCoroutine;

    public bool worShipper = false;
    public Image fillImage; // Health bar UI Image
    public GameObject worshipHolder;
    public GameObject bossKing, otherWorshop;
    public GameObject bossHealtHat;


    public bool isKing = false;
    private void Start()
    {
        if (spriteRenderer != null)
        {
            // Get a material instance to avoid modifying the shared material
            material = spriteRenderer.material;
        }
        maxHealth = health;
        // Initialize the health bar if the fillImage is assigned
        if (fillImage != null)
        {
            UpdateHealthBar();
        }

    }

    public void TakeDamage(int Damage)
    {
        health -= Damage;

        // Apply hit effect when taking damage
        TakeHit();

        // Update health bar if it's assigned
        if (fillImage != null)
        {
            UpdateHealthBar();
        }

        if (health <= 0)
        {
            Die();
        }
    }

    private void TakeHit()
    {
        if (hitEffectCoroutine != null)
        {
            StopCoroutine(hitEffectCoroutine);
        }
        hitEffectCoroutine = StartCoroutine(ApplyHitEffect());
    }

    private IEnumerator ApplyHitEffect()
    {
        float elapsedTime = 0f;
        material.SetFloat("_HitEffectBlend", maxHitEffect);

        // Gradually fade the effect
        while (elapsedTime < hitEffectDuration)
        {
            elapsedTime += Time.deltaTime;
            float blendValue = Mathf.Lerp(maxHitEffect, 0f, elapsedTime / hitEffectDuration);
            material.SetFloat("_HitEffectBlend", blendValue);
            yield return null;
        }

        // Ensure it fully resets
        material.SetFloat("_HitEffectBlend", 0f);
        hitEffectCoroutine = null;
    }

    private void Die()
    {
        if(theCoinPrefab != null)
        {
            float randomChance = Random.Range(0f, 1f); // Generate a random number between 0 and 1

            if (randomChance <= 0.5f) // 50% chance
            {
                Instantiate(theCoinPrefab, spawnPoint1.position, Quaternion.identity);
            }
            else if (randomChance <= 0.75f) // 25% chance
            {
                Instantiate(theCoinPrefab, spawnPoint1.position, Quaternion.identity);
                Instantiate(theCoinPrefab, spawnPoint2.position, Quaternion.identity);
            }
            else if (randomChance <= 0.85f) // 10% chance
            {
                Instantiate(theCoinPrefab, spawnPoint1.position, Quaternion.identity);
                Instantiate(theCoinPrefab, spawnPoint2.position, Quaternion.identity);
                Instantiate(theCoinPrefab, spawnPoint3.position, Quaternion.identity);
            }
        }

        if(isKing)
        {
            AudioManager.Instance.ChangeMusic("Theme", 1.5f, 1f);
            SceneManager.LoadScene("win");

        }

        if (worShipper)
        {
            worshipHolder.SetActive(false);
            if(otherWorshop == null)
                {
                bossKing.SetActive(true);
                bossHealtHat.SetActive(true);
            }
        }
        // No coin spawn for remaining 15%

        AudioManager.Instance.PlaySFX("COWFUCKINGDIES");
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        float healthPercent = Mathf.Clamp01((float)health / maxHealth);

        // Update the fill amount of the health bar
        fillImage.fillAmount = healthPercent;
    }
}
