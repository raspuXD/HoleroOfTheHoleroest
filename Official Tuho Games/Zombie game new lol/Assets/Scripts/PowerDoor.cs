using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerDoor : MonoBehaviour
{
    [SerializeField] PowerSwitch power;
    [SerializeField] Color color;
    UIChanger textChanger;

    private void Update()
    {
        if (power.isPowerOn)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textChanger = other.GetComponent<UIChanger>();
            textChanger.ChangeCrossHairColor(color);
            textChanger.WriteTheText("Must Turn Power On First", 0.04f);
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textChanger.UnWriteTheText(.02f);
            textChanger.ChangeCrossHairColor(Color.white);
        }
    }
}
