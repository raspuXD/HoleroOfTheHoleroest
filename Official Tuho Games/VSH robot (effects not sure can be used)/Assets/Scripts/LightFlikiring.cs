using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlikiring : MonoBehaviour
{
    [Header("Normal Mode")]
    [SerializeField] Light lightObject;
    [SerializeField] float PowerLight;

    [Header("Flicker Mode")]

    public float minLightValue;
    public float maxLightValue;
    [SerializeField] float minWaitTime, maxWaitTime;

    [SerializeField] bool isFlashlight = false;
    [SerializeField] float minRange, maxRange;

    public bool isPowerOn = true;

    Coroutine theFlas;
    private void Start()
    {
        if (!isFlashlight)
        {
            if (isPowerOn)
            {
                lightObject.intensity = PowerLight;
            }
            else
            {
                lightObject.intensity = maxLightValue;
            }
        }
        else
        {
            lightObject.intensity = maxLightValue;
            lightObject.spotAngle = maxRange;
        }
        theFlas = StartCoroutine(FlickerLight());
    }

    public void TheFlicge()
    {
        if(theFlas != null)
        {
            StopCoroutine(theFlas);
        }

        theFlas = StartCoroutine(FlickerLight());
    }

    IEnumerator FlickerLight()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            if(!isFlashlight)
            {
                if (isPowerOn)
                {
                    lightObject.intensity = PowerLight;
                }
                else
                {
                    if (lightObject.intensity == maxLightValue)
                    {
                        lightObject.intensity = minLightValue;
                    }
                    else
                    {
                        lightObject.intensity = maxLightValue;
                    }
                }
            }
            else
            {
                if (lightObject.intensity == maxLightValue)
                {
                    lightObject.intensity = minLightValue;
                    lightObject.spotAngle = minRange;
                }
                else
                {
                    lightObject.intensity = maxLightValue;
                    lightObject.spotAngle = maxRange;
                }
            }
        }
    }
}
