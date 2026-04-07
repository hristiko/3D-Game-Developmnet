using UnityEngine;
using UnityEngine.AI;

public class GoblinSpawner : MonoBehaviour
{
    public GameObject goblinPrefab;
    public Transform player;
    public Transform[] spawnPoints;

    public int goblinsPerWave = 3;
    public float timeBetweenWaves = 180f;

    public float navMeshSearchRadius = 2f;
    public float spawnSpread = 0f;

    int currentWaveIndex = 0;
    float nextWaveTime = 0f;

    public bool FinishedSpawning
    {
        get { return currentWaveIndex >= spawnPoints.Length; }
    }

    void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        SpawnWaveAtPoint(spawnPoints[currentWaveIndex]);
        currentWaveIndex++;

        nextWaveTime = Time.time + timeBetweenWaves;
    }

    void Update()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
            return;

        if (currentWaveIndex >= spawnPoints.Length)
            return;

        if (Time.time >= nextWaveTime)
        {
            SpawnWaveAtPoint(spawnPoints[currentWaveIndex]);
            currentWaveIndex++;

            nextWaveTime = Time.time + timeBetweenWaves;
        }
    }

    void SpawnWaveAtPoint(Transform spawnPoint)
    {
        for (int i = 0; i < goblinsPerWave; i++)
        {
            Vector3 spawnPosition = spawnPoint.position;

            if (spawnSpread > 0f)
            {
                Vector2 randomOffset = Random.insideUnitCircle * spawnSpread;
                spawnPosition += new Vector3(randomOffset.x, 0f, randomOffset.y);
            }

            GameObject newGoblin = Instantiate(
                goblinPrefab,
                spawnPosition,
                spawnPoint.rotation
            );

            GoblinBehaviour goblinBehaviour = newGoblin.GetComponent<GoblinBehaviour>();
            if (goblinBehaviour != null)
                goblinBehaviour.player = player;
        }
    }
}