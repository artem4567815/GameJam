using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData
{
    public string weaponName;
    public Sprite icon;
    [Tooltip("Текущее количество патронов (меняется в игре)")] public int currentAmmo;
    [Tooltip("Стартовое количество патронов из префаба")] public int startAmmo;
    public BulletData bulletSettings;

    public WeaponData() { }

    public WeaponData(WeaponData other)
    {
        weaponName = other.weaponName;
        icon = other.icon;
        startAmmo = other.startAmmo;
        currentAmmo = other.startAmmo;
        bulletSettings = other.bulletSettings != null ? ScriptableObject.Instantiate(other.bulletSettings) : null;
    }

    public static WeaponData Default()
    {
        return new WeaponData
        {
            weaponName = "default",
            icon = Resources.Load<Sprite>("3"),
            currentAmmo = -1,
            startAmmo = -1,
            bulletSettings = Resources.Load<BulletData>("defaut rifle")
        };
    }
}
