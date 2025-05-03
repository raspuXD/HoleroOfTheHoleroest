using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarpasLatka : MonoBehaviour
{
    public BoxCollider theCollider;
    public Animator animas;

    public bool canHit = false;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        canHit = true;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
        {
            if (canHit)
            {
                canHit = false;
                animas.SetTrigger("Hit");
                StartCoroutine(HitAgain());
            }
        }
    }

    IEnumerator HitAgain()
    {
        yield return new WaitForSeconds(.2f);
        theCollider.enabled = true;
        yield return new WaitForSeconds(.5f);
        theCollider.enabled = false;
        yield return new WaitForSeconds(.3f);
        canHit = true;
    }
}
