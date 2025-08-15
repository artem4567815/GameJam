using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public int[] enemyCounts;
    public Transform player;
    public float spawnRadius = 10f;
    public float spawnInterval = 2f;
    public float minDistance = 4f; 

    private float timer;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= HandlePlayerDeath;
    }

    void HandlePlayerDeath()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(enemy);
        }
    }

    void Start()
    {
        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            int maxCount = (enemyCounts != null && i < enemyCounts.Length) ? enemyCounts[i] : 1;
            for (int j = 0; j < maxCount; j++)
            {
                SpawnEnemy(i);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            List<int> availableIndexes = new List<int>();
            for (int i = 0; i < enemyPrefabs.Length; i++)
            {
                int currentCount = CountEnemiesOfType(enemyPrefabs[i]);
                int maxCount = enemyCounts[i];
                if (currentCount < maxCount)
                {
                    availableIndexes.Add(i);
                }
            }
            if (availableIndexes.Count > 0)
            {
                int randomIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
                SpawnEnemy(randomIndex);
                timer = 0f;
            }
        }
    }

    void SpawnEnemy(int index)
    {
        if (enemyPrefabs.Length == 0 || index < 0 || index >= enemyPrefabs.Length)
        {
            return;
        }

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minDistance, spawnRadius);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawnPos = (Vector2)player.position + offset;

        GameObject enemyToSpawn = enemyPrefabs[index];
        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
    }

    int CountEnemiesOfType(GameObject prefab)
    {
        int count = 0;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.name.Contains(prefab.name))
            {
                count++;
            }
        }
        return count;
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

}
