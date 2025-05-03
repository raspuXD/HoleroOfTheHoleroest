using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Booster : MonoBehaviour
{
    public ParticleSystem explo1, explo2, explo3, explo4;
    public Rigidbody theCarRB;
    public float force = 5000f;
    private bool canBoost = true;
    private float cooldownTime = 10f;
    public Car theCar;
    public Image fillImage;
    public TMP_Text theText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canBoost && theCar.isPlayerInTheCar)
        {
            LaunchTheCar();
        }
    }

    void LaunchTheCar()
    {
        theCarRB.AddForce(transform.forward * force, ForceMode.Impulse);
        AudioManager.Instance.PlaySFX("Explo");
        explo1.Play();
        explo2.Play();
        explo3.Play();
        explo4.Play();

        canBoost = false;
        StartCoroutine(ResetBoost());
    }

    IEnumerator ResetBoost()
    {
        float elapsedTime = 0f;
        theText.text = "";
        while (elapsedTime < cooldownTime)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = elapsedTime / cooldownTime;
            yield return null;
        }
        canBoost = true;
        fillImage.fillAmount = 1f; // Fully refilled at the end
        theText.text = "Boost";
    }
}
