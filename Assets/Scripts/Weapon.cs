using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public WeaponData data;

    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public BulletData GetBulletData()
    {
        return data.bulletSettings;
    }

}
