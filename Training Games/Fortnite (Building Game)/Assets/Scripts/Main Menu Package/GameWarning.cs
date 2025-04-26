using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameWarning : MonoBehaviour
{
    public float howLongWait = 5f;
    public GameObject theCloseButton;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(howLongWait);
        theCloseButton.SetActive(true);
    }

}
