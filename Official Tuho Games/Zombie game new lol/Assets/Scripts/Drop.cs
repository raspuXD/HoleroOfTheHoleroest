using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    CurrentDrops drops;
    public bool instaKill;
    public bool doublePoints;
    public bool Nuke;
    int id;

    private IEnumerator Start()
    {
        if(instaKill)
        {
            id = 0;
        }
        else if(doublePoints)
        {
            id = 1;
        }
        else 
        {
            id = 2;
        }

        yield return new WaitForSeconds(20f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CurrentDrops droppersa = FindObjectOfType<CurrentDrops>();
            droppersa.ActivateSome(id);
            Destroy(gameObject);
        }
    }
}
