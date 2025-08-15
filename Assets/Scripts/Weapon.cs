using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public WeaponData data;

    private void Awake()
    {
        float ammoMult = 1f;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var buffs = player.GetComponent<PlayerBuffs>();
            if (buffs != null)
                ammoMult = buffs.maxAmmoMultiplier;
        }
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
                data.currentAmmo = Mathf.RoundToInt(data.startAmmo * ammoMult);
            }
            else
            {
                data.currentAmmo = -1;
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
