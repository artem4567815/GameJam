using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public WeaponSlot slot;

    private PlayerInventory inventory;
    private WeaponManager manager;
    private WeaponData weaponData;
    private GameObject gun;

    void Start()
    {
        currentHealth = maxHealth;

        gun = slot.weaponPrefab;
        Weapon weapon = gun.GetComponent<Weapon>();
        weaponData = weapon.data;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
            manager = player.GetComponent <WeaponManager>();
        }
        else
        {
            Debug.LogWarning("Игрок не найден в сцене!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " получил урон: " + damage + " | HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        int slotIndex = inventory.GetSlotIndex();
        if (slotIndex == 0) 
        {
            return;
        }

        bool changeWeapon;
        changeWeapon = inventory.AddWeapon(weaponData);
        if (changeWeapon)
        {
            manager.UpdateWeaponSlot(slotIndex, slot);
        }
    }
}
