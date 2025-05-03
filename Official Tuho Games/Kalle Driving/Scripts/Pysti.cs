using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pysti : MonoBehaviour
{
    public NPC thePetteri;
    bool onAntaunu = false;
    public GameObject theEffect;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Mitsu") && !onAntaunu)
        {
            onAntaunu = true;
            thePetteri.howManyCurrentlyCollected++;
            Destroy(gameObject);
            Instantiate(theEffect, transform.position, Quaternion.identity);
        }
        if (other.CompareTag("Player") && !onAntaunu)
        {
            onAntaunu = true;
            AudioManager.Instance.PlaySFX("Coin");
            thePetteri.howManyCurrentlyCollected++;
            Destroy(gameObject);
            Instantiate(theEffect, transform.position, Quaternion.identity);
        }
    }
}
