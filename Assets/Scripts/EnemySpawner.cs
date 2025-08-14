using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;  // ������ �������� ������
    public Transform player;
    public float spawnRadius = 10f;
    public float spawnInterval = 2f;
    public int maxEnemies = 10;
    public float minDistance = 4f; // Минимальная дистанция до игрока

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval && CountEnemies() < maxEnemies)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            return;
        }

        // Генерируем точку на кольце между minDistance и spawnRadius одной попыткой
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minDistance, spawnRadius);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawnPos = (Vector2)player.position + offset;

        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyToSpawn = enemyPrefabs[randomIndex];

        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

}
