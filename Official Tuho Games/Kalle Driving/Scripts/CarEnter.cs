using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CarEnter : MonoBehaviour
{
    public GameObject[] activate, deactivate;

    bool canEnter = false;
    public Car theCar;
    public GameObject getIn;
    public GameObject getOut;
    public Transform wherePlayer;
    public bool isOkToLeave = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (canEnter)
            {
                DoStuff(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!theCar.isPlayerInTheCar)
        {
            if(other.CompareTag("Player"))
            {
                canEnter = true;
                getIn.SetActive(true);
            }
        }

        if(other.CompareTag("Ground"))
        {
            isOkToLeave = false;
        }
    }

    public void DoStuff(bool how)
    {
        if(how)
        {
            canEnter = false;
            theCar.isPlayerInTheCar = true;
            theCar.hasBeenInCarForSecond = false;
            theCar.StartACountDown();
            getIn.SetActive(false);

            foreach (GameObject go in activate)
            {
                go.SetActive(true);
            }

            foreach (GameObject go in deactivate)
            {
                go.SetActive(false);
            }
        }
        else
        {
            theCar.isPlayerInTheCar = false;
            getOut.SetActive(false);

            foreach (GameObject go in activate)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in deactivate)
            {
                go.SetActive(true);
            }

            deactivate[1].transform.position = wherePlayer.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!theCar.isPlayerInTheCar)
        {
            if (other.CompareTag("Player"))
            {
                canEnter = false;
                getIn.SetActive(false);
            }
        }

        if (other.CompareTag("Ground"))
        {
            isOkToLeave = true;
        }
    }
}
