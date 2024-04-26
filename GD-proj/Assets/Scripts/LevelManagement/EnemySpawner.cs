using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyPrefab2;
    public float spawnInterval = 5f;
    public float spawnRadius = 40f;
    public float minDistanceFromPlayer = 30f;

    public LevelManagement levelManagement;
    private Transform player;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private Coroutine checkEnemiesCoroutine;

    private int remainingEnemies;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //StartCoroutine(SpawnEnemies());
        SpawnEnemies();
        //checkEnemiesCoroutine = StartCoroutine(CheckEnemiesCoroutine());
    }


    private void SpawnEnemies()
    {
        int currentLevel = levelManagement.GetCurrentLevel();

        int totalEnemiesToSpawn = currentLevel * 2 + 1;
        totalEnemiesToSpawn = Mathf.Min(totalEnemiesToSpawn, 13);

        // idk lol
        int numPrefab1 = totalEnemiesToSpawn / 2;
        int numPrefab2 = totalEnemiesToSpawn - numPrefab1;

        Debug.Log("Spawning " + totalEnemiesToSpawn + " enemies in total");
        Debug.Log("Spawning " + numPrefab1 + " of " + enemyPrefab.name + " and " + numPrefab2 + " of " +
                  enemyPrefab2.name);

        remainingEnemies = totalEnemiesToSpawn;

        for (int i = 0; i < numPrefab1; i++)
        {
            SpawnEnemy(enemyPrefab);
        }

        for (int i = 0; i < numPrefab2; i++)
        {
            SpawnEnemy(enemyPrefab2);
        }
    }

    private void SpawnEnemy(GameObject enemyPrefabSpawnable)
    {
        Vector3 spawnPosition = GetRandomPositionOnNavMesh(transform.position);

        if (spawnPosition != Vector3.zero)
        {
            GameObject enemy = Instantiate(enemyPrefabSpawnable, spawnPosition, Quaternion.identity);
            spawnedEnemies.Add(enemy);

            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();
            SubscribeToEnemyDeath(enemyScript);

            enemy.GetComponent<CharacterController>().enabled = true;
            Destroy(enemy.GetComponent<Rigidbody>());

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.enabled = true;

                agent.SetDestination(player.position);
            }
        }
    }

    private Vector3 GetRandomPositionOnNavMesh(Vector3 center)
    {
        Vector3 randomDirection;
        NavMeshHit hit;
        int attempts = 0;
        const int maxAttempts = 10;

        do
        {
            randomDirection = Random.insideUnitSphere * spawnRadius;
            randomDirection += center;

            float distanceToPlayer = Vector3.Distance(randomDirection, player.position);

            if (distanceToPlayer > minDistanceFromPlayer)
            {
                if (NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            attempts++;
        } while (attempts < maxAttempts);

        return Vector3.zero;
    }

    private void SubscribeToEnemyDeath(EnemyScript enemyScript)
    {
        enemyScript.OnDeath += HandleEnemyDeath;
    }

    private void HandleEnemyDeath(GameObject enemyGameObject)
    {
        remainingEnemies--;
        Debug.Log("Remaining enemies: " + remainingEnemies);
        if (remainingEnemies <= 0)
        {
            levelManagement.CompleteLevel();
        }
    }
}