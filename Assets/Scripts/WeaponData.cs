using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public Sprite icon;
    public int currentAmmo;
    public BulletData bulletSettings;

    public static WeaponData Default()
    {
        return new WeaponData
        {
            weaponName = "default",
            icon = Resources.Load<Sprite>("3"),
            currentAmmo = -1
        };
    }
}
