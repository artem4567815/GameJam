using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;         
    public float followDistance = 5f;
    public int damage = 10;
    public float attackDelay = 1f;         // Задержка между ударами (секунды)
    public float attackDistance = 1f;

    private Transform player;
    private EnemyHealth health;
    private PlayerHealth playerHealth;

    private float lastAttackTime = -Mathf.Infinity;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<EnemyHealth>();
        playerHealth = player.GetComponent<PlayerHealth>();

    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= followDistance || health.currentHealth < health.maxHealth)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
        }

        if (distance <= attackDistance && Time.time >= lastAttackTime + attackDelay)
        {
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }

    }


}
