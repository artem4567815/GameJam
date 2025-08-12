using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private WeaponData[] weaponSlots = new WeaponData[4];
    public int currentWeaponIndex = 0;

    public WeaponUI weaponUI;

    public WeaponSlot defaultSlot;
    public WeaponManager weaponManager;


    private void Awake()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            GameObject data = weaponManager.weaponSlots[i].weaponPrefab;
            Weapon weaponData = data.GetComponent<Weapon>();
            weaponSlots[i] = weaponData.data;
        }
        weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
        Debug.Log(weaponSlots);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) NextWeapon();
        else if (scroll < 0f) PreviousWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeapon(2);

        if (weaponSlots[currentWeaponIndex].currentAmmo <= 0 && weaponSlots[currentWeaponIndex].currentAmmo != -1)
        {
            weaponSlots[currentWeaponIndex] = WeaponData.Default();
            weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
            weaponManager.UpdateWeaponSlot(currentWeaponIndex, defaultSlot);
        }
    }

    void NextWeapon()
    {
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponSlots.Length;
        weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
    }

    void PreviousWeapon()
    {
        currentWeaponIndex--;
        if (currentWeaponIndex < 0) currentWeaponIndex = weaponSlots.Length - 1;
        weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
    }

    void SetWeapon(int index)
    {
        if (index >= 0 && index < weaponSlots.Length)
        {
            currentWeaponIndex = index;
            weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
        }
    }

    public void UpdateAmmo()
    {
        if (weaponSlots[currentWeaponIndex].currentAmmo > 0)
        {
            weaponSlots[currentWeaponIndex].currentAmmo--;
            weaponUI.UpdateAmmo(weaponSlots[currentWeaponIndex]);
        }
    }

    public int SetAmmo()
    {
        return weaponSlots[currentWeaponIndex].currentAmmo;
    }

    public bool AddWeapon(WeaponData newWeapon)
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (i != currentWeaponIndex && weaponSlots[i].weaponName == newWeapon.weaponName)
            {
                return false;
            }
        }

        WeaponData currentWeapon = weaponSlots[currentWeaponIndex];
        Debug.Log(currentWeapon.weaponName);
        Debug.Log(newWeapon.weaponName);


        if (currentWeapon == null || currentWeapon.weaponName == "default")
        {
            weaponSlots[currentWeaponIndex] = newWeapon;
            weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
            return true;
        }
        else if (currentWeapon.weaponName == newWeapon.weaponName)
        {
            currentWeapon.currentAmmo += newWeapon.currentAmmo;
            weaponUI.UpdateAmmo(currentWeapon);
            return false;
        }
        else
        {
            return false;
        }
    }

    public int GetSlotIndex()
    {
        return currentWeaponIndex;
    }

    public void SetDefaultWeapon()
    {
        for (int i = 1; i < weaponSlots.Length; i++)
        {
            weaponSlots[i] = WeaponData.Default();
            weaponUI.UpdateWeaponSlots(weaponSlots, i);
            weaponManager.UpdateWeaponSlot(i, defaultSlot);
        } 
    }

}
