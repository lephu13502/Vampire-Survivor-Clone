using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;

    float spawnTimer;
    public float waveInterval;
    public int enemyAlive;
    public int maxEnemyAllowed;
    public bool maxEnemyReached = false;

    public List<Transform> relativeSpawnPoints;
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    private void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0)
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
    }

    private void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }
        waves[currentWaveCount].waveQuota = currentWaveQuota;
    }

    private void SpawnEnemies()
    {
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemyReached)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemyAlive >= maxEnemyAllowed)
                    {
                        maxEnemyReached = true;
                        return;
                    }

                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemyAlive++;
                }
            }
        }
        if (enemyAlive < maxEnemyAllowed)
        {
            maxEnemyReached = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemyAlive--;
    }
}
