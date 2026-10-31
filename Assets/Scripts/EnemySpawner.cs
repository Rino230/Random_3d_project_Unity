using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text enemyCounterText;

    [Header("Spawn Settings")]
    [SerializeField] private int enemiesToSpawn = 5;
    [SerializeField] private float minSpawnDistance = 15f;
    [SerializeField] private float maxSpawnDistance = 25f;
    [SerializeField] private float spawnInterval = 2f;

    [Header("Arena Bounds")]
    [SerializeField] private float minX = -23f;
    [SerializeField] private float maxX = 23f;
    [SerializeField] private float minZ = -23f;
    [SerializeField] private float maxZ = 23f;

    private int spawnedEnemies;
    private int aliveEnemies;

    private void Start()
    {
        enemyCounterText.text = "Âðàãè: " + enemiesToSpawn;

        InvokeRepeating(nameof(SpawnEnemy), 1f, spawnInterval);
    }

    private void SpawnEnemy()
    {
        if (spawnedEnemies >= enemiesToSpawn)
        {
            CancelInvoke(nameof(SpawnEnemy));
            return;
        }

        if (TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            GameObject newEnemy = Instantiate(
                enemyPrefab,
                spawnPosition,
                Quaternion.identity
            );

            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();

            if (enemyAI != null)
                enemyAI.Initialize(player);

            EnemyHealth enemyHealth = newEnemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
                enemyHealth.OnDeath += OnEnemyDeath;

            spawnedEnemies++;
            aliveEnemies++;
        }
    }

    private void OnEnemyDeath()
    {
        aliveEnemies--;

        int remainingEnemies = enemiesToSpawn - (spawnedEnemies - aliveEnemies);

        enemyCounterText.text = "Âðàãè: " + remainingEnemies;

        if (spawnedEnemies >= enemiesToSpawn && aliveEnemies <= 0)
            SpawnBoss();
    }

    private void SpawnBoss()
    {
        Debug.Log("Boss incoming!");
        enemyCounterText.text = "ÁÎÑÑ!";

        if (TryGetSpawnPosition(out Vector3 spawnPosition))
        {
            GameObject newBoss = Instantiate(
                bossPrefab,
                spawnPosition,
                Quaternion.identity
            );

            EnemyAI enemyAI = newBoss.GetComponent<EnemyAI>();

            if (enemyAI != null)
                enemyAI.Initialize(player);
        }
    }

    private bool TryGetSpawnPosition(out Vector3 spawnPosition)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(minX, maxX),
                0f,
                Random.Range(minZ, maxZ)
            );

            float distance = Vector3.Distance(player.position, randomPosition);

            if (distance < minSpawnDistance || distance > maxSpawnDistance)
                continue;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                float actualDistance = Vector3.Distance(player.position, hit.position);

                if (actualDistance < minSpawnDistance || actualDistance > maxSpawnDistance)
                    continue;

                spawnPosition = hit.position;
                return true;
            }
        }

        spawnPosition = Vector3.zero;
        Debug.LogWarning("Could not find a suitable spawn position.");
        return false;
    }
}