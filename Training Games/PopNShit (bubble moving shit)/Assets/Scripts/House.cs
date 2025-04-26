using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class House : MonoBehaviour
{
    public LayerMask targetLayerMask;
    public bool isHouse = true;
    public RASNUSCUTSCENE rasnusCutScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("canPickUp"))
        {
            AudioManager.Instance.PlaySFX("Bubble");

            if (isHouse)
            {
                PlayerPrefs.SetInt("LoseEnding", 1);
                rasnusCutScene.BadEnding();
            }

            Destroy(gameObject);
        }
    }
}
