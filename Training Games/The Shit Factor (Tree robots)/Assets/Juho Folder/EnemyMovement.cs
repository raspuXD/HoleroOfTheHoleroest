using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;

    public float currentHealht;
    public float maxHealth = 20;
    public float updateInterval = 0.75f;

    private Rigidbody2D rb;
    public RareModel model;
    public bool isHiding = false;
    public ParticleSystem damageParticleLot, damageSmall;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target").transform;
        rb = GetComponent<Rigidbody2D>();
        currentHealht = maxHealth;
        StartCoroutine(UpdateVelocity());
    }

    public void TakeDamage(int damage)
    {
        currentHealht -= damage;

        if(damage == 1)
        {
            damageSmall.Play();
        }
        else
        {
            damageParticleLot.Play();
        }

        if (currentHealht <= 0)
        {
            AudioManager.Instance.PlaySFX("au");
            Destroy(gameObject);
        }
    }

    IEnumerator UpdateVelocity()
    {
        while (true)
        {
            if (model != null)
            {
                model.HideLol(1);

                if (Random.value < 0.25f)
                {
                    AudioManager.Instance.PlaySFX("Bush");
                }

                isHiding = false;
            }

            if (target != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * moveSpeed;
            }

            yield return new WaitForSeconds(2f);

            if (model != null)
            {
                model.HideLol(0);
                rb.velocity = Vector2.zero;
                isHiding = true;
            }

            yield return new WaitForSeconds(updateInterval - 2f);
        }
    }

}
