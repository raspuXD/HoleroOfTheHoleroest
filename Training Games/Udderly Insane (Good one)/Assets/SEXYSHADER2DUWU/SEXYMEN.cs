using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEXYMEN : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer; // Assign in Inspector
    [SerializeField] private float hitEffectDuration = 0.5f; // Effect time
    [SerializeField] private float maxHitEffect = 1.0f; // Max blend value

    private Material material;
    private Coroutine hitEffectCoroutine;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            // Get a material instance to avoid modifying the shared material
            material = spriteRenderer.material;
        }
    }

    public void TakeHit()
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
}
