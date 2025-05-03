using UnityEngine;
using System.Collections;

public class WindowRepair : MonoBehaviour
{
    public GameObject[] theBoards;
    private bool canRepair;
    public bool isRepairing;
    private Coroutine repairCoroutine;
    private bool allBoardsActive;
    public int howMuchHasGiven = 0;
    private UIChanger uiChanger;
    [SerializeField] Color color;
    [SerializeField] Points points;

    private void Update()
    {
        if (canRepair && Input.GetKeyDown(KeyCode.E) && !isRepairing)
        {
            repairCoroutine = StartCoroutine(RepairLastInactiveBoard());
        }

        if (isRepairing && Input.GetKeyUp(KeyCode.E))
        {
            StopCoroutine(repairCoroutine);
            isRepairing = false;
        }
    }

    private IEnumerator RepairLastInactiveBoard()
    {
        isRepairing = true;

        while (true)
        {
            allBoardsActive = true;

            for (int i = theBoards.Length - 1; i >= 0; i--)
            {
                if (!theBoards[i].activeSelf)
                {
                    yield return new WaitForSeconds(1.25f);
                    theBoards[i].SetActive(true);
                    HandelMoney();
                    allBoardsActive = false;
                    break;
                }
            }

            if (allBoardsActive)
            {
                uiChanger.UnWriteTheText(.02f);
                uiChanger.ChangeCrossHairColor(Color.white);
                break;
            }
        }

        isRepairing = false;
    }

    void HandelMoney()
    {
        if(howMuchHasGiven < 50)
        {
            points.EarnPoints(10);
            howMuchHasGiven = howMuchHasGiven + 10;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(uiChanger == null)
            {
                uiChanger = other.GetComponent<UIChanger>();
            }

            if (!AreAllBoardsActive() && canRepair == false)
            {
                uiChanger.WriteTheText("Rebuild Boards By Holding E", 0.035f);
                uiChanger.ChangeCrossHairColor(color);
                canRepair = true;
            }
        }
    }

    private bool AreAllBoardsActive()
    {
        foreach (GameObject board in theBoards)
        {
            if (!board.activeSelf)
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canRepair = false;
            uiChanger.UnWriteTheText(.02f);
            uiChanger.ChangeCrossHairColor(Color.white);

            if (isRepairing)
            {
                StopCoroutine(repairCoroutine);
                isRepairing = false;
            }
        }
    }
}
