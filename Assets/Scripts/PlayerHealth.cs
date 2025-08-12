using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private Vector3 startPosition;
    private PlayerInventory inventory;
    public WeaponUI ui;

    void Start()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;
        inventory = GetComponent<PlayerInventory>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ui.UpdateHP(currentHealth, maxHealth);
        Debug.Log("Игрок получил урон: " + damage + " | HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Игрок умер. Перезапуск...");
        transform.position = startPosition;
        currentHealth = maxHealth;
        ui.UpdateHP(currentHealth, maxHealth);
        inventory.SetDefaultWeapon();
    }

}
