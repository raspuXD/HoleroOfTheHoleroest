using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public Image fillImage;

    [SerializeField] private SpriteRenderer spriteRenderer; // Assign in Inspector
    [SerializeField] private float hitEffectDuration = 0.5f; // Effect time
    [SerializeField] private float maxHitEffect = 1.0f; // Max blend value

    private Material material;
    private Coroutine hitEffectCoroutine;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (spriteRenderer != null)
        {
            material = spriteRenderer.material;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log("Player Health: " + currentHealth);
        TakeHit();

        if(currentHealth <= 0)
        {
            AudioManager.Instance.ChangeMusic("Theme", 1.5f, 1f);
            SceneManager.LoadScene("gameOver");
        }
    }

    public void RestoreHealth(int amount)
    {
        currentHealth += amount; // Increase health by the specified amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp to max health
        UpdateHealthUI();
        Debug.Log("Health Restored: " + currentHealth);
    }

    void UpdateHealthUI()
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = (float)currentHealth / maxHealth;
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

        while (elapsedTime < hitEffectDuration)
        {
            elapsedTime += Time.deltaTime;
            float blendValue = Mathf.Lerp(maxHitEffect, 0f, elapsedTime / hitEffectDuration);
            material.SetFloat("_HitEffectBlend", blendValue);
            yield return null;
        }

        material.SetFloat("_HitEffectBlend", 0f);
        hitEffectCoroutine = null;
    }
}
