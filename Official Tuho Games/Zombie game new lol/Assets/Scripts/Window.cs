using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Window : MonoBehaviour
{
    public GameObject[] theBoards;
    public bool allBoardsGone = false;

    [SerializeField] WindowRepair theRepair;

    private List<Collider> attackingZombies = new List<Collider>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie") && !attackingZombies.Contains(other))
        {
            attackingZombies.Add(other);
            StartCoroutine(ZombieAttacksCoroutine(other));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            attackingZombies.Remove(other);
        }
    }

    public void RemoveABoard()
    {
        for (int i = theBoards.Length - 1; i >= 0; i--)
        {
            if (theBoards[i].activeSelf)
            {
                theBoards[i].SetActive(false);
                break;
            }
        }
    }

    private IEnumerator ZombieAttacksCoroutine(Collider zombieCollider)
    {
        NavMeshAgent zombieNav = zombieCollider.GetComponent<NavMeshAgent>();
        ZombieFollow zombieFollow = zombieCollider.GetComponent<ZombieFollow>();
        zombieNav.enabled = false;
        zombieFollow.enabled = false;

        while (attackingZombies.Contains(zombieCollider))
        {
            if (zombieCollider == null || !zombieCollider.gameObject.activeSelf)
            {
                attackingZombies.Remove(zombieCollider);
                yield break;
            }

            yield return new WaitForSeconds(1.4f);

            RemoveABoard();

            allBoardsGone = true;
            foreach (GameObject board in theBoards)
            {
                if (board.activeSelf)
                {
                    allBoardsGone = false;
                    break;
                }
            }

            if (allBoardsGone)
            {
                yield return new WaitForSeconds(1.4f);

                if (!theRepair.isRepairing)
                {
                    zombieNav.enabled = true;
                    zombieFollow.enabled = true;
                    attackingZombies.Remove(zombieCollider);
                    yield break;
                }
            }
        }

        // Restore zombie movement if it leaves the trigger before the window is fully broken
        zombieNav.enabled = true;
        zombieFollow.enabled = true;
    }
}
