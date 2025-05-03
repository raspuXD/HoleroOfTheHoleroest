using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GranadeManager : MonoBehaviour
{
    [Header("GrenadeSpawn")]
    public int howManyGrenades = 0;
    [SerializeField] float throwForce = 10;
    [SerializeField] Granade grenadePrefab;

    [Header("UI")]
    public Image theSkull;
    [SerializeField] TMP_Text textAmount;

    [SerializeField] Sprite skull0, skull1, skull2, skull3, skull4;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) && howManyGrenades > 0)
        {
            ThrowGrenade();
        }
    }
    void ThrowGrenade()
    {
        // Instantiate the grenade at the throw point
        Granade grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);

        // Apply a forward force to the grenade's Rigidbody to throw it
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * throwForce, ForceMode.VelocityChange);
        }

        // Decrease the grenade count
        IncreaseGrenades(-1);
    }


    public void IncreaseGrenades(int howMany)
    {
        howManyGrenades = Mathf.Clamp(howManyGrenades + howMany, 0, 4);
        CheckWhatSkull();
    }

    void CheckWhatSkull()
    {
        StartCoroutine(SmoothTransition());
    }

    IEnumerator SmoothTransition()
    {
        int targetGrenades = Mathf.Clamp(howManyGrenades, 0, 4);
        int currentGrenades = GetCurrentGrenades();

        while (currentGrenades != targetGrenades)
        {
            if (currentGrenades < targetGrenades)
                currentGrenades++;
            else if (currentGrenades > targetGrenades)
                currentGrenades--;

            UpdateSkullImage(currentGrenades);
            yield return new WaitForSeconds(0.4f);
        }
    }

    int GetCurrentGrenades()
    {
        if (theSkull.sprite == skull0) return 0;
        else if (theSkull.sprite == skull1) return 1;
        else if (theSkull.sprite == skull2) return 2;
        else if (theSkull.sprite == skull3) return 3;
        else if (theSkull.sprite == skull4) return 4;
        return 0;
    }

    void UpdateSkullImage(int grenades)
    {
        switch (grenades)
        {
            case 0:
                theSkull.sprite = skull0;
                textAmount.text = "0";
                break;
            case 1:
                theSkull.sprite = skull1;
                textAmount.text = "1";
                break;
            case 2:
                theSkull.sprite = skull2;
                textAmount.text = "2";
                break;
            case 3:
                theSkull.sprite = skull3;
                textAmount.text = "3";
                break;
            case 4:
                theSkull.sprite = skull4;
                textAmount.text = "4";
                break;
        }
    }
}
