using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public WeaponData data;

    private void Awake()
    {
        if (data != null)
        {
            // Если стартовое значение не задано (0) и это не бесконечные (-1), берем из currentAmmo из инспектора
            if (data.startAmmo == 0 && data.currentAmmo > 0)
            {
                data.startAmmo = data.currentAmmo;
            }
            // Всегда сбрасываем на старт при создании экземпляра оружия
            if (data.startAmmo != -1)
            {
                data.currentAmmo = data.startAmmo;
            }
            else
            {
                data.currentAmmo = -1; // бесконечные
            }
        }
    }

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public BulletData GetBulletData()
    {
        return data.bulletSettings;
    }

}
