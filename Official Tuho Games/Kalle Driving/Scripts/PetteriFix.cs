using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetteriFix : MonoBehaviour
{
    public Car theCar;
    public RagdollMover ragdoll;
    public GameObject player, playerCanvas, animatorGameOb;
    public AnimationClip theClip;
    bool canPlay = true;
    bool isPlaying = false;
    Coroutine animationLol;
    Coroutine coolDownCor;
    public float coolDown = 5f;
    public CarDurability dura;
    public CarFlip flip;
    public TurretEnter turrot;
    public GameObject theRagdollFirst = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PetteriRagdoll") && !theCar.isPlayerInTheCar && !isPlaying && !ragdoll.isHolding && canPlay && !turrot.isInTheTurret)
        {
            canPlay = false;
            isPlaying = true;
            flip.isInAnimation = true;
            theRagdollFirst = other.transform.root.gameObject;
            theRagdollFirst.SetActive(false);
            player.SetActive(false);
            playerCanvas.SetActive(false);

            if (animationLol != null)
            {
                StopCoroutine(animationLol);
            }
            animationLol = StartCoroutine(WaitUntilAnimationOver());
        }
    }

    IEnumerator WaitUntilAnimationOver()
    {
        animatorGameOb.SetActive(true);
        yield return new WaitForSeconds(theClip.length);
        dura.currentDura++;
        theRagdollFirst.SetActive(true);
        theRagdollFirst = null;
        animatorGameOb.SetActive(false);
        isPlaying = false;
        playerCanvas.SetActive(true);
        player.SetActive(true);
        flip.isInAnimation = false;


        if (coolDownCor != null)
        {
            StopCoroutine(coolDownCor);
        }
        coolDownCor = StartCoroutine(CoolDownLol());
    }

    IEnumerator CoolDownLol()
    {
        yield return new WaitForSeconds(coolDown);
        canPlay = true;
    }
}
