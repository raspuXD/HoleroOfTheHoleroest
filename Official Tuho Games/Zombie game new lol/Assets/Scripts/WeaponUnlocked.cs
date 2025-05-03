using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUnlocked : MonoBehaviour
{
    public bool isWeaponUnlocked = false;
    [SerializeField] Weapon weapon;

    public void UpdateUI()
    {
        weapon.UpdateUI();
        weapon.arm.MoveBack();
    }
}
