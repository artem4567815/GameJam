using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Action<float, float> OnHealthChanged;
    public float maxHealth = 100;
    public float currentHealth;
    public WeaponSlot slot;

    private PlayerInventory inventory;
    private WeaponManager manager;
    private WeaponData weaponData; // данные для дропа
    private GameObject gun;
    public static event System.Action<string> OnEnemyKilled;
    public static event System.Action<GameObject> OnEnemyKilledGameObject;

    private string race;

    void Start()
    {
        currentHealth = maxHealth;
        race = gameObject.name;
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

    public void TakeDamage(float damage)
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
        OnEnemyKilled?.Invoke(gameObject.name);

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
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
}
