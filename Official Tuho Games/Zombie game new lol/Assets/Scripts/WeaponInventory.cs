using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private WeaponUnlocked[] weapons;
    private int currentWeaponIndex = 0;
    private float weaponChangeCooldown = 0.5f; // Time in seconds to wait before allowing another weapon change
    private float nextWeaponChangeTime = 0f; // The time when the next weapon change is allowed

    private void Start()
    {
        SelectWeapon(0);
    }

    void Update()
    {
        // Check if the cooldown period has passed
        if (Time.time < nextWeaponChangeTime)
            return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f)
        {
            SelectNextWeapon();
        }
        else if (scroll < 0f)
        {
            SelectPreviousWeapon();
        }

        for (int i = 1; i <= weapons.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int weaponIndex = i - 1;
                if (weapons[weaponIndex].isWeaponUnlocked)
                {
                    SelectWeapon(weaponIndex);
                }
            }
        }
    }

    void SelectNextWeapon()
    {
        int originalIndex = currentWeaponIndex;
        do
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Length;
        }
        while (!weapons[currentWeaponIndex].isWeaponUnlocked && currentWeaponIndex != originalIndex);

        SelectWeapon(currentWeaponIndex);
    }

    void SelectPreviousWeapon()
    {
        int originalIndex = currentWeaponIndex;
        do
        {
            currentWeaponIndex = (currentWeaponIndex - 1 + weapons.Length) % weapons.Length;
        }
        while (!weapons[currentWeaponIndex].isWeaponUnlocked && currentWeaponIndex != originalIndex);

        SelectWeapon(currentWeaponIndex);
    }

    public void SelectWeapon(int index)
    {
        // Set all weapons inactive first
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }

        // Update the current weapon index and activate the selected weapon
        currentWeaponIndex = index;
        weapons[currentWeaponIndex].gameObject.SetActive(true);
        weapons[currentWeaponIndex].UpdateUI();

        // Set the time when the next weapon change is allowed
        nextWeaponChangeTime = Time.time + weaponChangeCooldown;
    }
}
