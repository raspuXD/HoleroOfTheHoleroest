using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterBuilding : MonoBehaviour
{
    bool isNear;
    public string sceneName;
    public GameObject getIn;
    public SceneHandler sceneHandler;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isNear)
            {
                sceneHandler.LoadSceneNamed(sceneName);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = true;
            getIn.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isNear = false;
            getIn.SetActive(false);
        }
    }
}
