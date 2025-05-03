using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petteri : MonoBehaviour
{
    public GameObject theRagdoll;
    private MoneyManager moneyManager;
    private bool hasSpawned;
    public Animator theAnim;
    public string playerTag = "Player";
    public float detectionRadius = 20f;
    public Transform[] theWalkingPoints;
    public float walkSpeed = 4f, runSpeed = 6f;
    private float currentSpeed;
    private int currentPointIndex = 0;
    private NPC[] allNPCs;

    private void Start()
    {
        allNPCs = FindObjectsOfType<NPC>();
        moneyManager = FindObjectOfType<MoneyManager>(); // Cache the MoneyManager
        InvokeRepeating(nameof(CheckPlayerDistance), 0f, 1f);
        currentSpeed = walkSpeed;
        StartCoroutine(WalkPath());
    }

    private void CheckPlayerDistance()
    {
        bool playerNearby = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(playerTag))
            {
                playerNearby = true;
                break;
            }
        }

        theAnim.SetBool("needsToRun", playerNearby);
        currentSpeed = playerNearby ? runSpeed : walkSpeed;
    }

    private IEnumerator WalkPath()
    {
        while (currentPointIndex < theWalkingPoints.Length)
        {
            Transform targetPoint = theWalkingPoints[currentPointIndex];

            while (Vector3.Distance(transform.position, targetPoint.position) > 0.2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, currentSpeed * Time.deltaTime);
                transform.LookAt(targetPoint.position);
                yield return null;
            }

            currentPointIndex++;
        }

        Destroy(gameObject);
    }

    public void SetStartIndex(int index)
    {
        currentPointIndex = Mathf.Min(index + 1, theWalkingPoints.Length - 1);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasSpawned)
        {
            if (other.CompareTag("Mitsu"))
            {
                CarDurability dura = FindObjectOfType<CarDurability>();
                if (dura != null)
                {
                    dura.currentDura--;
                }

                Car theCar = FindObjectOfType<Car>();
                if(theCar.isPlayerInTheCar && theCar.currentSpeedKmh > 70f)
                {
                    AudioManager.Instance.PlaySFX("Bone");
                }

                moneyManager.GivePetteriMoney(1);
                HandleDeath(true, false);
            }
            else if (other.CompareTag("Swatter"))
            {
                PlaySwatterSound();
                moneyManager.GivePetteriMoney(.5f);
                HandleDeath(false, true);
            }
            else if(other.CompareTag("SilentKiller"))
            {
                moneyManager.GivePetteriMoney(.5f);
                HandleDeath(false, false);
            }
        }
    }

    private void PlaySwatterSound()
    {
        int theChance = Random.Range(0, 101);
        if (theChance < 90)
        {
            AudioManager.Instance.PlaySFX("Swatter1");
        }
        else if (theChance < 95)
        {
            AudioManager.Instance.PlaySFX("Swatter2");
        }
        else
        {
            AudioManager.Instance.PlaySFX("Swatter3");
        }
    }

    private void HandleDeath(bool isByCar, bool isBySwatter)
    {
        hasSpawned = true;

        if(isByCar)
        {
            foreach (NPC theOne in allNPCs)
            {
                if (theOne.mitsullaPetterinPaalta && theOne.isMissionOn)
                {
                    theOne.montaMitsullaTapettu++;
                }
            }
        }

        if (isBySwatter)
        {
            foreach (NPC theOne in allNPCs)
            {
                if (theOne.karpasLatkallaPetteria && theOne.isMissionOn)
                {
                    theOne.montaKarpasLatkallaTapettu++;
                }
            }
        }

        Instantiate(theRagdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}