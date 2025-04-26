using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfFamily : MonoBehaviour
{
    public GameObject person1, person2, person3;
    public RASNUSCUTSCENE rasnuscutscene;
    void Update()
    {
        if(person1 == null && person2 == null && person3 == null)
        {
            //do the cutscene
            
            //NormalEnding
            PlayerPrefs.SetInt("MonsterEnding", 1);
            rasnuscutscene.KillEnding();
        }
    }
}
