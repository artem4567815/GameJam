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

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += LoseAmmoOnDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= LoseAmmoOnDeath;
    }

    private void Awake()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            GameObject data = weaponManager.weaponSlots[i].weaponPrefab;
            Weapon weaponData = data.GetComponent<Weapon>();
            weaponSlots[i] = new WeaponData(weaponData.data);
        }
        weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0f) NextWeapon();
        else if (scroll < 0f) PreviousWeapon();

        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeapon(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetWeapon(3);

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
            Debug.Log(weaponSlots[currentWeaponIndex].currentAmmo);
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

        if (currentWeapon == null || currentWeapon.weaponName == "default")
        {
            weaponSlots[currentWeaponIndex] = new WeaponData(newWeapon);
            weaponUI.UpdateWeaponSlots(weaponSlots, currentWeaponIndex);
            return true;
        }
        else if (currentWeapon.weaponName == newWeapon.weaponName)
        {
            int add = newWeapon.startAmmo == -1 ? 0 : newWeapon.startAmmo;
            if (currentWeapon.currentAmmo != -1)
                currentWeapon.currentAmmo += add;
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

    public void LoseAmmoOnDeath()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            var weapon = weaponSlots[i];
            if (weapon != null && weapon.weaponName != "default" && weapon.currentAmmo > 0)
            {
                int lost = Mathf.FloorToInt(weapon.currentAmmo * 0.75f);
                weapon.currentAmmo -= lost;
                weaponUI.UpdateAmmo(weapon);
            }
        }
    }
    
}
