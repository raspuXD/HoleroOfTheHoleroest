using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyParticle : MonoBehaviour
{
    public float min, max;
    Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetTrigger("Bubble");
    }

    private IEnumerator Start()
    {
        while (true)
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            float theWaitTime = Random.Range(min, max);

            yield return new WaitForSeconds(theWaitTime);

            animator.SetTrigger("Bubble");
        }
    }
}
