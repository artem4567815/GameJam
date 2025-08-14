using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform currentWeapon;
    private Transform firePoint;
    public PlayerInventory inventory;
    private BulletData bulletSettings;

    public float nextFireTime = 0f;


    void Update()
    {
        if (currentWeapon != null && (firePoint == null || firePoint != currentWeapon.Find("FirePoint")))
        {
            firePoint = currentWeapon.Find("FirePoint");
        }

        Weapon weapon = currentWeapon.GetComponent<Weapon>();
        if (weapon != null)
        {
            bulletSettings = weapon.GetBulletData();
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextFireTime)
            {
                int ammo = inventory.SetAmmo();
                if (ammo > 0 || ammo == -1)
                {
                    Shoot();
                    inventory.UpdateAmmo();
                    nextFireTime = Time.time + bulletSettings.fireRate;
                }
            }
        }
    }

    void Shoot()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 baseDirection = mousePos - firePoint.position;

        for (int i = 0; i < bulletSettings.bulletsCount; i++)
        {
            Vector2 finalDirection;

            if (bulletSettings.accuracy >= 100f)
            {
                float randomAngle = Random.Range(0f, 360f);
                finalDirection = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad));
            }
            else if (bulletSettings.accuracy > 0f)
            {
                float maxAngleOffset = bulletSettings.accuracy * 1.8f; // 100% = ~180�
                float angleOffset = Random.Range(-maxAngleOffset, maxAngleOffset);
                Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
                finalDirection = rotation * baseDirection.normalized;
            }
            else
            {
                finalDirection = baseDirection.normalized;
            }

            GameObject bulletObj = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Bullet bullet = bulletObj.GetComponent<Bullet>();

            bullet.speed = bulletSettings.speed;
            bullet.damage = bulletSettings.damage;
            bullet.fireRate = 0f;
            bullet.fireRange = bulletSettings.fireRange;
            bullet.accuracy = bulletSettings.accuracy;
            bullet.bulletsCount = bulletSettings.bulletsCount;

            bullet.SetDirection(finalDirection);
        }
    }

}
