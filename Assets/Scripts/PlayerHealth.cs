using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event System.Action OnPlayerDeath;
    public float maxHealth = 100f;
    public float currentHealth;

    private PlayerBuffs buffs;
    private Vector3 startPosition;
    private PlayerInventory inventory;
    public WeaponUI ui;

    void Start()
    {
        buffs = GetComponent<PlayerBuffs>();
        float healthMult = buffs != null ? buffs.healthMultiplier : 1f;
        currentHealth = maxHealth * healthMult;
        startPosition = transform.position;
        inventory = GetComponent<PlayerInventory>();
    }

    public void TakeDamage(int damage)
    {
        float healthMult = buffs != null ? buffs.healthMultiplier : 1f;
        currentHealth -= damage;
        ui.UpdateHP(currentHealth, maxHealth * healthMult);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        float healthMult = buffs != null ? buffs.healthMultiplier : 1f;
        OnPlayerDeath?.Invoke();
        transform.position = startPosition;
        currentHealth = maxHealth * healthMult;
        ui.UpdateHP(currentHealth, maxHealth * healthMult);
    }

}
