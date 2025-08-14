using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Action<int, int> OnHealthChanged;
    public int maxHealth = 100;
    public int currentHealth;
    public WeaponSlot slot;

    private PlayerInventory inventory;
    private WeaponManager manager;
    private WeaponData weaponData; // данные для дропа
    private GameObject gun;

    void Start()
    {
    currentHealth = maxHealth;
    OnHealthChanged?.Invoke(currentHealth, maxHealth);

        gun = slot.weaponPrefab;
        if (gun == null)
        {
            Debug.Log("плаки плаки!!!2");
            return;
        }
        Weapon weapon = gun.GetComponent<Weapon>();
        if (weapon == null)
        {
            Debug.Log("плаки плаки!!!");
            return;
        }
        // Создаём чистую копию данных (WeaponData конструктор сам сбросит currentAmmo в startAmmo)
        weaponData = new WeaponData(weapon.data);


        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
            manager = player.GetComponent <WeaponManager>();
        }
        else
        {
            Debug.LogWarning("Player не найден в сцене!");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " получил урон: " + damage + " | HP: " + currentHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
    Destroy(gameObject);
    OnHealthChanged?.Invoke(0, maxHealth);
        int slotIndex = inventory.GetSlotIndex();
        if (slotIndex == 0) 
        {
            return;
        }

        bool changeWeapon = inventory.AddWeapon(weaponData);
        if (changeWeapon)
        {
            manager.UpdateWeaponSlot(slotIndex, slot);
        }
    }
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
}
