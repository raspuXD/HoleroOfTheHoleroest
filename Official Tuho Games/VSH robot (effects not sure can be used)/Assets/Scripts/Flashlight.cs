using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Flashlight : MonoBehaviour
{
    [SerializeField] GameObject lightObject;
    [SerializeField] LightFlikiring flikiring;
    bool isOn = false;
    public AudioSource activation;
    public AudioSource idle;
    private float idleTimer = 0f;
    private float idleInterval;
    float oldRange;
    private float lastToggleTime = -2f;
    private float toggleCooldown = 2f;
    [SerializeField] Image cooldownUI;
    [SerializeField] TMP_Text theActivationText;

    private void Start()
    {
        oldRange = flikiring.minLightValue;
    }

    void Update()
    {
        float cooldownProgress = Mathf.Clamp01((Time.time - lastToggleTime) / toggleCooldown);
        cooldownUI.fillAmount = cooldownProgress;

        bool canToggle = Time.time - lastToggleTime >= toggleCooldown;
        theActivationText.text = canToggle ? "F" : "";

        if (Input.GetKeyDown(KeyCode.F) && canToggle)
        {
            lastToggleTime = Time.time;

            if (isOn)
            {
                lightObject.SetActive(false);
                isOn = false;
                activation.Play();
            }
            else
            {
                lightObject.SetActive(true);
                flikiring.TheFlicge();
                flikiring.minLightValue = oldRange;
                isOn = true;
                activation.Play();
            }
        }

        if (isOn)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleInterval)
            {
                idle.Play();
                StopAllCoroutines();
                StartCoroutine(flicker());
                idleTimer = 0f;
                idleInterval = Random.Range(7f, 10f);
            }
        }
    }

    IEnumerator flicker()
    {
        yield return new WaitForSeconds(.8f);
        flikiring.minLightValue = flikiring.minLightValue - 2f;

        yield return new WaitForSeconds(3f);
        flikiring.minLightValue = oldRange;
    }
}