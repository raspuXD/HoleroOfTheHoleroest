using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckAllItems : MonoBehaviour
{
    public List<GameObject> allFurniture;
    public List<GameObject> allItemsInScene;

    private void Start()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("canPickUp");
        allItemsInScene.AddRange(items);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("canPickUp") && !allFurniture.Contains(other.gameObject))
        {
            allFurniture.Add(other.gameObject);
        }

        if (AllItemsCollected())
        {
            Debug.Log("holero");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("canPickUp") && allFurniture.Contains(other.gameObject))
        {
            allFurniture.Remove(other.gameObject);
        }
    }

    private bool AllItemsCollected()
    {
        foreach (GameObject item in allItemsInScene)
        {
            if (!allFurniture.Contains(item))
            {
                return false;
            }
        }
        return true;
    }
}
