using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RASNUSCUTSCENE : MonoBehaviour
{
    public PlayableDirector badEnding; // Reference to the PlayableDirector
    public PlayableDirector goodEnding;
    public PlayableDirector killEnding;



    public void BadEnding()
    {
        if (badEnding != null)
        {
            badEnding.Play(); // Starts playing the Timeline
            Debug.Log("Timeline started.");
        }
    }
    public void GoodEnding()
    {
        if (goodEnding != null)
        {
            goodEnding.Play(); // Starts playing the Timeline
            Debug.Log("Timeline started.");
        }
    }
    public void KillEnding()
    {
        if (killEnding != null)
        {
            killEnding.Play(); // Starts playing the Timeline
            Debug.Log("Timeline started.");
        }
    }
}
