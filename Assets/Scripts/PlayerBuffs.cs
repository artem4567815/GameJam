using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[DisallowMultipleComponent]
public class PlayerBuffs : MonoBehaviour
{
    public static event System.Action<float> OnMaxHealthChanged;

    [Header("Множители характеристик игрока")]
    public float healthMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;
    public float maxAmmoMultiplier = 1f;

    private HashSet<string> buffedRaces = new HashSet<string>();

    private void OnEnable()
    {
        EnemyHealth.OnEnemyKilled += OnEnemyKilled;
    }
    private void OnDisable()
    {
        EnemyHealth.OnEnemyKilled -= OnEnemyKilled;
    }

    private void OnEnemyKilled(string enemyName)
    {
        if (!buffedRaces.Contains(enemyName))
        {
            healthMultiplier += healthMultiplier * 0.075f;
            damageMultiplier += damageMultiplier * 0.05f;
            speedMultiplier += speedMultiplier * 0.03f;
            maxAmmoMultiplier += maxAmmoMultiplier * 0.03f;
            buffedRaces.Add(enemyName);

            OnMaxHealthChanged?.Invoke(healthMultiplier * 100);
        }
        Debug.Log($"Buffs updated: Health x{healthMultiplier}, Damage x{damageMultiplier}, Speed x{speedMultiplier}, Max Ammo x{maxAmmoMultiplier}");
    }

    public void SetHealthMultiplier(float value) => healthMultiplier = value;
    public void SetDamageMultiplier(float value) => damageMultiplier = value;
    public void SetSpeedMultiplier(float value) => speedMultiplier = value;
    public void SetMaxAmmoMultiplier(float value) => maxAmmoMultiplier = value;
}
