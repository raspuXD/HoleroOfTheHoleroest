using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoesPartMiss : MonoBehaviour
{
    public GameObject thePartThasIsMissing;
    public GameObject theCameraObject;
    ToolInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<ToolInventory>();
    }
    public bool IsThePartPut()
    {
        if(!thePartThasIsMissing.activeSelf)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!thePartThasIsMissing.activeSelf)
        {
            if (other.CompareTag("PartTrigger"))
            {
                thePartThasIsMissing.SetActive(true);
                theCameraObject.SetActive(false);
                inventory.holdsPart = false;
            }
        }
    }
}
