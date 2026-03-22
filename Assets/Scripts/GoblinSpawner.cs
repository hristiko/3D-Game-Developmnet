using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinSpawner : MonoBehaviour
{
    public GameObject goblinPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    public int totalGoblins = 9;
    public int goblinsPerWave = 3;
    public float timeBetweenWaves = 180f;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        int spawnedCount = 0;

        while (spawnedCount < totalGoblins && spawnedCount < spawnPoints.Length)
        {
            int goblinsThisWave = goblinsPerWave;

            if (spawnedCount + goblinsThisWave > totalGoblins)
                goblinsThisWave = totalGoblins - spawnedCount;

            if (spawnedCount + goblinsThisWave > spawnPoints.Length)
                goblinsThisWave = spawnPoints.Length - spawnedCount;

            for (int i = 0; i < goblinsThisWave; i++)
            {
                Transform spawnPoint = spawnPoints[spawnedCount];

                GameObject newGoblin = Instantiate(
                    goblinPrefab,
                    spawnPoint.position + Vector3.up * 1f,
                    spawnPoint.rotation
                );

                NavMeshAgent agent = newGoblin.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                        agent.Warp(hit.position);
                }

                GoblinBehaviour goblinBehaviour = newGoblin.GetComponent<GoblinBehaviour>();
                if (goblinBehaviour != null)
                    goblinBehaviour.player = player;

                spawnedCount++;
            }

            if (spawnedCount >= totalGoblins || spawnedCount >= spawnPoints.Length)
                yield break;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }
}