using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryShow : MonoBehaviour
{
    public GameObject theCanvas;
    Coroutine closing;

    private IEnumerator Start()
    {
        ActivateThis();
        yield return new WaitForSeconds(1f);
        HideInventory();

    }
    public void ActivateThis()
    {
        theCanvas.SetActive(true);
    }

    public void HideInventory()
    {
        if(closing != null)
        {
            StopCoroutine(closing);
        }
        closing = StartCoroutine(ClosingHEHE());
    }

    IEnumerator ClosingHEHE()
    {
        yield return new WaitForSeconds(2f);

        theCanvas.SetActive(false);
    }
}
