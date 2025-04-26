using System.Collections;
using UnityEngine;

public class CowShit : MonoBehaviour
{
    public float slowMultiplier = 0.5f; // 50% speed reduction
    public float slowDuration = 2f; // Duration after leaving
    private bool isPlayerInTrap = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TopViewPlayer player = other.GetComponent<TopViewPlayer>();
        if (player != null && !isPlayerInTrap) 
        {
            AudioManager.Instance.PlaySFX("SHAAT");
            StartCoroutine(ApplySlowEffect(player));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TopViewPlayer player = other.GetComponent<TopViewPlayer>();
        if (player != null)
        {
            StartCoroutine(ContinueSlowAfterExit(player));
            isPlayerInTrap = false;
        }
    }

    private IEnumerator ApplySlowEffect(TopViewPlayer player)
    {
        isPlayerInTrap = true;
        float originalSpeed = player.speed;
        float originalRunSpeed = player.runSpeed;

        player.speed *= slowMultiplier; 
        player.runSpeed *= slowMultiplier - 0.25f;
        player.canSprint = false;

        while (isPlayerInTrap) // Keep applying effect while in trap
        {
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator ContinueSlowAfterExit(TopViewPlayer player)
    {
        yield return new WaitForSeconds(slowDuration); // Effect persists after leaving

        player.speed = 5f; // Restore original speed
        player.runSpeed = 8f;
        player.canSprint = true;
    }
}
