using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;

    public RoundManager roundManager;
    public Points points;
    public CurrentDrops currentDrops;

    int howMuchHasGivenAlready = 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if(currentDrops.isInstakill)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }

        if(health <= 0)
        {
            points.EarnPoints(90);
            roundManager.theZombies.Remove(this);
            Destroy(gameObject);
        }
        else
        {
            if(howMuchHasGivenAlready < 90)
            {
                points.EarnPoints(10);
                howMuchHasGivenAlready = howMuchHasGivenAlready + 10;
            }
        }
    }
}
