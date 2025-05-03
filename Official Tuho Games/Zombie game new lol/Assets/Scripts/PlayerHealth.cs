using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth = 100f;
    public float maxPlayerHealth = 100f;
    public float regenRate = 1;
    bool canRegen = false;

    public float healthCooldown = 2f;
    public float maxHealCooldown = 2f;
    bool startcooldown = false;

    [SerializeField] Image splatter;
    [SerializeField] SceneController sceneController;

    void UpdateHealth()
    {
        Color splatterAplha = splatter.color;
        splatterAplha.a = 1 - (currentHealth / maxPlayerHealth);
        splatter.color = splatterAplha;
    }

    public void TakeDamage(float amount)
    {
        canRegen = false;
        currentHealth -= amount;
        UpdateHealth();
        healthCooldown = maxHealCooldown;
        startcooldown = true;

        if(currentHealth <= 0)
        {
            sceneController.LoadScene("MainMenu");
        }
    }

    private void Update()
    {
        if(startcooldown)
        {
            healthCooldown -= Time.deltaTime;
            if(healthCooldown <= 0)
            {
                canRegen = true;
                startcooldown = false;
            }
        }

        if(canRegen)
        {
            if(currentHealth <= maxPlayerHealth - 0.01)
            {
                currentHealth += Time.deltaTime * regenRate;
                UpdateHealth();
            }
            else
            {
                currentHealth = maxPlayerHealth;
                healthCooldown = maxHealCooldown;
                canRegen = false;
            }
        }
    }
}
