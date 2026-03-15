using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalEnemySpawner : MonoBehaviour
{
    /// <summary>
    /// 적 검병 프리팹
    /// </summary>
    public GameObject swordSoldierPrefab;

    /// <summary>
    /// 적 방패병 프리팹
    /// </summary>
    public GameObject shieldSoldierPrefab;

    /// <summary>
    /// 적 활병 프리팹
    /// </summary>
    public GameObject archerSoldierPrefab;

    /// <summary>
    /// 적 프리팹 배열
    /// </summary>
    private List<GameObject> enemyPrefabs;

    /// <summary>
    /// 적 검병 트랜스폼
    /// </summary>
    public Transform swordSoldierTransform;

    /// <summary>
    /// 적 방패병 트랜스폼
    /// </summary>
    public Transform shieldSoldierTransform;

    /// <summary>
    /// 적 활병 트랜스폼
    /// </summary>
    public Transform archerSoldierTransform;

    /// <summary>
    /// 적 스폰 트랜스폼 배열
    /// </summary>
    private List<Transform> enemySpawnPoints;

    /// <summary>
    /// 적 스폰 최소 간격
    /// </summary>
    public float minSpawnInterval = 0.25f;

    /// <summary>
    /// 적 스폰 최대 간격
    /// </summary>
    public float maxSpawnInterval = 1.0f;

    /// <summary>
    /// 적 스폰 수
    /// </summary>
    public int enemySpawnCount = 10;

    /// <summary>
    /// 적 스폰 상태
    /// </summary>
    private bool isOnSpawn = false;

    private void Start()
    {
        enemyPrefabs = new List<GameObject>();
        enemyPrefabs.Add(swordSoldierPrefab);
        enemyPrefabs.Add(shieldSoldierPrefab);
        enemyPrefabs.Add(archerSoldierPrefab);

        enemySpawnPoints = new List<Transform>();
        enemySpawnPoints.Add(swordSoldierTransform);
        enemySpawnPoints.Add(shieldSoldierTransform);
        enemySpawnPoints.Add(archerSoldierTransform);

        StartSpawn();
    }

    private void StartSpawn()
    {
        if (isOnSpawn)
        {
            return;
        }

        StartCoroutine(SpawnCoroutine());
    }

    private int GetRandomEnemyIndex()
    {
        return Random.Range(0, enemyPrefabs.Count);
    }

    private IEnumerator SpawnCoroutine()
    {
        isOnSpawn = true;

        for (int i = 0; i < enemySpawnCount; i++)
        {
            int index = GetRandomEnemyIndex();
            GameObject enemyPrefab = enemyPrefabs[index];
            Transform enemyTransform = enemySpawnPoints[index];
            Instantiate(enemyPrefab, enemyTransform.position, Quaternion.identity);

            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }

        isOnSpawn = false;
    }
}
