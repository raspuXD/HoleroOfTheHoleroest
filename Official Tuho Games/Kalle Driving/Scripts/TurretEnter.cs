using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnter : MonoBehaviour
{
    public GameObject[] activate, deactivate;

    bool isNear;
    public bool isInTheTurret = false;
    public GameObject getIn;
    public GameObject getOut;
    public PlayerMovement movement;
    public RotateTurret turretRo;
    public LineRenderer lineRenderer;
    public Crosshair hair;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(isNear)
            {
                if (isInTheTurret)
                {
                    DoStuff(true);
                    hair.ChangeTheCursor(false);
                }
                else
                {
                    DoStuff(false);
                    hair.ChangeTheCursor(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            isInTheTurret = true;
            getIn.SetActive(true);
        }
    }

    public void DoStuff(bool how)
    {
        if (how)
        {
            movement.enabled = false;
            isInTheTurret = false;
            turretRo.enabled = true;
            getIn.SetActive(false);
            getOut.SetActive(true);

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
            movement.enabled=true;
            turretRo.ResetRotation();
            turretRo.enabled=false;
            getOut.SetActive(false);
            lineRenderer.enabled=false;

            foreach (GameObject go in activate)
            {
                go.SetActive(false);
            }

            foreach (GameObject go in deactivate)
            {
                go.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear=false;
            isInTheTurret = false;
            getIn.SetActive(false);
        }
    }
}
