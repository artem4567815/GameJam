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
    private List<GameObject> spawned = new List<GameObject>();

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
        // Destroy only enemies spawned by this spawner
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] != null)
                Destroy(spawned[i]);
            spawned.RemoveAt(i);
        }

        // Respawn only for this spawner
        RespawnAll();
    }

    void RespawnAll()
{
    if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;
    for (int i = 0; i < enemyPrefabs.Length; i++)
    {
        int maxCount = (enemyCounts != null && i < enemyCounts.Length) ? enemyCounts[i] : 1;
        for (int j = 0; j < maxCount; j++)
            SpawnEnemy(i);
    }
}

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

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
                int maxCount = (enemyCounts != null && i < enemyCounts.Length) ? enemyCounts[i] : 1;
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
        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || index < 0 || index >= enemyPrefabs.Length)
        {
            return;
        }

        GameObject enemyToSpawn = enemyPrefabs[index];

        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minDistance, spawnRadius);
        Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * distance;
        Vector2 spawnPos = (Vector2)transform.position + offset;

        if (player != null && Vector2.Distance(player.position, transform.position) <= spawnRadius)
        {
            float distToPlayer = Vector2.Distance(spawnPos, player.position);
            if (distToPlayer < minDistance)
            {
                Vector2 dir = (spawnPos - (Vector2)player.position).normalized;
                if (dir == Vector2.zero)
                {
                    dir = new Vector2(Mathf.Cos(angle + 0.5f), Mathf.Sin(angle + 0.5f)).normalized;
                }
                spawnPos = (Vector2)player.position + dir * minDistance;

                float distToSpawner = Vector2.Distance(spawnPos, transform.position);
                if (distToSpawner > spawnRadius)
                {
                    Vector2 dirFromSpawner = (spawnPos - (Vector2)transform.position).normalized;
                    spawnPos = (Vector2)transform.position + dirFromSpawner * spawnRadius * 0.99f;
                }
            }
        }

    GameObject go = Instantiate(enemyToSpawn, (Vector3)spawnPos, Quaternion.identity, this.transform);
    spawned.Add(go);
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
