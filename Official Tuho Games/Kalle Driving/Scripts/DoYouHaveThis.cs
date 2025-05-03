using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoYouHaveThis : MonoBehaviour
{
    public string thePlayerPrefs;
    private void Start()
    {
        int hasIt = PlayerPrefs.GetInt(thePlayerPrefs, 0);
        if(hasIt == 0)
        {
            Destroy(gameObject);
        }
    }
}
