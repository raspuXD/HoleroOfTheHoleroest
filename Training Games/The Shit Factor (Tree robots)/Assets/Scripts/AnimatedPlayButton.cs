using System.Collections;
using UnityEngine;

public class AnimatedPlayButton : MonoBehaviour
{
    SceneHandler theSceneChanger;
    Coroutine theCoroutine;
    bool hasStarted = false;
    public GameObject theObjectToActiavrte;
    public AnimationClip theAnimationClip;

    public void PlayLOL(string name)
    {
        if(!hasStarted)
        {
            hasStarted = true;  
            theObjectToActiavrte.SetActive(true);
            theCoroutine = StartCoroutine(WaitLol(name));
        }

    }

    IEnumerator WaitLol(string name)
    {
        yield return new WaitForSeconds(theAnimationClip.length);
        theSceneChanger = FindObjectOfType<SceneHandler>();
        theSceneChanger.LoadSceneNamed(name);
    }
}
