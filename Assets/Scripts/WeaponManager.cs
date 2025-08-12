using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WeaponSlot
{
    public GameObject weaponPrefab;
}

public class WeaponManager : MonoBehaviour
{

    [Header("Настройки")]
    public Transform weaponHolder;
    public WeaponSlot[] weaponSlots; 

    private GameObject currentWeapon;
    public PlayerShooting shooting;

    public void EquipWeaponFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= weaponSlots.Length)
            return;

        WeaponSlot slot = weaponSlots[slotIndex];

        if (slot.weaponPrefab == null)
        {
            RemoveWeapon();
            return;
        }

        RemoveWeapon();

        currentWeapon = Instantiate(slot.weaponPrefab, weaponHolder);
        currentWeapon.transform.localPosition = Vector3.zero;
        currentWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        shooting.currentWeapon = currentWeapon.transform;
    }

    private void RemoveWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
    }

    public void UpdateWeaponSlot(int slotIndex, WeaponSlot newWeaponSlot)
    {
        weaponSlots[slotIndex] = newWeaponSlot;
        EquipWeaponFromSlot(slotIndex);
    }

}
