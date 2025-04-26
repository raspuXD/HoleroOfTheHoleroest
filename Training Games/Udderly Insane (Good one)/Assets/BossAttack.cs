using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public TakeDamageZone[] attackZone;
    private int lastZoneIndex = -1;

    private void Start()
    {
        StartCoroutine(ActivateAttackZones());
    }

    private IEnumerator ActivateAttackZones()
    {
        while (true)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, attackZone.Length);
            }
            while (randomIndex == lastZoneIndex); // Ensure the same zone isn't picked twice

            lastZoneIndex = randomIndex; // Update the last used zone index
            TakeDamageZone randomZone = attackZone[randomIndex];

            randomZone.colliderr.enabled = false;
            randomZone.canDeal = false;
            randomZone.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);

            randomZone.colliderr.enabled = true;
            randomZone.canDeal = true;

            yield return new WaitForSeconds(.25f);

            randomZone.gameObject.SetActive(false);

            yield return new WaitForSeconds(2f);
        }
    }
}
