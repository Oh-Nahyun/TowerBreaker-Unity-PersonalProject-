using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{
    /// <summary>
    /// 적 물속성 보스 프리팹
    /// </summary>
    public GameObject waterSoldierPrefab;

    /// <summary>
    /// 적 빛속성 보스 프리팹
    /// </summary>
    public GameObject lightSoldierPrefab;

    /// <summary>
    /// 적 프리팹 배열
    /// </summary>
    private List<GameObject> enemyPrefabs;

    /// <summary>
    /// 적 물속성 보스 트랜스폼
    /// </summary>
    public Transform waterSoldierTransform;

    /// <summary>
    /// 적 빛속성 보스 트랜스폼
    /// </summary>
    public Transform lightSoldierTransform;

    /// <summary>
    /// 적 스폰 트랜스폼 배열
    /// </summary>
    private List<Transform> enemySpawnPoints;

    /// <summary>
    /// 적 스폰 최소 간격
    /// </summary>
    public float minSpawnInterval = 2.0f;

    /// <summary>
    /// 적 스폰 최대 간격
    /// </summary>
    public float maxSpawnInterval = 3.0f;

    /// <summary>
    /// 적 스폰 수
    /// </summary>
    public int enemySpawnCount = 5;

    /// <summary>
    /// 적 스폰 상태
    /// </summary>
    private bool isOnSpawn = false;

    private void Start()
    {
        enemyPrefabs = new List<GameObject>();
        enemyPrefabs.Add(waterSoldierPrefab);
        enemyPrefabs.Add(lightSoldierPrefab);

        enemySpawnPoints = new List<Transform>();
        enemySpawnPoints.Add(waterSoldierTransform);
        enemySpawnPoints.Add(lightSoldierTransform);

        StartSpawn();
    }

    public void StartSpawn()
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
