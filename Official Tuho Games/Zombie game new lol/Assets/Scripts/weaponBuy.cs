using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponBuy : MonoBehaviour
{
    [SerializeField] int weaponID = 0;
    [SerializeField] int weaponCost;
    [SerializeField] WeaponInventory weaponInventory;
    [SerializeField] WeaponUnlocked weaponUnlocked;
    [SerializeField] Points points;

    [SerializeField] Color color;
    UIChanger textChanger;
    bool canBuy = false;

    private void Update()
    {
        //change to use custom keys
        if (canBuy && Input.GetKeyDown(KeyCode.E) && points.points >= weaponCost)
        {
            BuyShit();
            textChanger.UnWriteTheText(.02f);
            textChanger.ChangeCrossHairColor(Color.white);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textChanger = other.GetComponent<UIChanger>();

            textChanger.ChangeCrossHairColor(color);
            textChanger.WriteTheText("Buy Weapon Name For " + weaponCost.ToString(), 0.04f);
            canBuy = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textChanger.UnWriteTheText(.02f);
            textChanger.ChangeCrossHairColor(Color.white);
            canBuy = false;
        }
    }

    void BuyShit()
    {
        points.BuySomething(weaponCost);
        weaponUnlocked.isWeaponUnlocked = true;
        weaponInventory.SelectWeapon(weaponID);
        Destroy(gameObject);
    }
}
