using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConverBelt : MonoBehaviour
{
    MeshRenderer theMesh;
    public Material newMaterial;
    public AudioSource soundMaker;
    bool hasStartedToPlay = false;
    public float howFast = -.5f;


    private void Start()
    {
        theMesh = GetComponent<MeshRenderer>();
        newMaterial = theMesh.material;
        theMesh.material = newMaterial;
    }

    public void ChangeSpeed(bool doesItMove)
    {
        // Get the current value of the "Direction" property
        Vector2 direction = newMaterial.GetVector("_Direction");

        if (doesItMove)
        {
            // Set y to 0.12 if it should move
            direction.y = howFast;
            if(!hasStartedToPlay)
            {
                soundMaker.Play();
                hasStartedToPlay = true;
            }
        }
        else
        {
            // Set y to 0 if it should stop
            direction.y = 0f;
            soundMaker.Stop();
            hasStartedToPlay = false;
        }

        // Apply the updated direction to the material
        newMaterial.SetVector("_Direction", direction);
    }
}
