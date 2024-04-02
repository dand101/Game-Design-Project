using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnInterval = 5f;
    public float spawnRadius = 20f;
    private Transform player;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            Vector3 spawnPosition = GetRandomPositionOnNavMesh(transform.position);

            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();

            if (agent != null)
            {
                agent.enabled = true;
                yield return new WaitForSeconds(0.1f);
                agent.SetDestination(player.position);
            }
        }
    }

    private Vector3 GetRandomPositionOnNavMesh(Vector3 center)
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;
        randomDirection += center;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, NavMesh.AllAreas);
        return hit.position;
    }
}