using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectPart : MonoBehaviour
{
    public GameObject thePartInCamera;
    public bool canCollect;
    public ToolInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<ToolInventory>();
    }

    private void Update()
    {
        if(canCollect && inventory.IsOpenHanded)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                CollectTheItem();
                canCollect = false;
            }
        }
    }

    public void CollectTheItem()
    {
        inventory.holdsPart = true;
        thePartInCamera.SetActive(true);
    }

    public void PutTheItemAway()
    {
        thePartInCamera.SetActive(false);
    }
}
