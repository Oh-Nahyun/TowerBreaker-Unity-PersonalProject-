using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 인게임 패널
    /// </summary>
    public InGamePanel InGamePanel;

    /// <summary>
    /// 현재 스테이지 번호
    /// </summary>
    public int currentStage = 1;

    /// <summary>
    /// 현재 스테이지 적의 수
    /// </summary>
    public int totalEnemyCount;

    /// <summary>
    /// 플레이어가 죽인 적의 수
    /// </summary>
    public int deadEnemyCount;

    /// <summary>
    /// 일반 적 스포너
    /// </summary>
    public NormalEnemySpawner normalEnemySpawner;

    /// <summary>
    /// 보스 적 스포너
    /// </summary>
    public BossEnemySpawner bossEnemySpawner;

    public Player Player
    {
        get
        {
            if (player == null)
            {
                OnInitialize();
            }
            return player;
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        player = FindAnyObjectByType<Player>();
        InGamePanel = FindAnyObjectByType<InGamePanel>();

        InGamePanel.UpdateGameStageUI(currentStage);

        normalEnemySpawner = FindAnyObjectByType<NormalEnemySpawner>();
        bossEnemySpawner = FindAnyObjectByType<BossEnemySpawner>();

        totalEnemyCount = normalEnemySpawner.enemySpawnCount + bossEnemySpawner.enemySpawnCount;
    }

    public void CountDeadEnemy()
    {
        deadEnemyCount++;

        if (deadEnemyCount >= totalEnemyCount)
        {
            currentStage++;
            StartNewStage();
        }
    }

    public void StartNewStage()
    {
        deadEnemyCount = 0;
        totalEnemyCount = normalEnemySpawner.enemySpawnCount + bossEnemySpawner.enemySpawnCount;

        InGamePanel.UpdateGameStageUI(currentStage);

        normalEnemySpawner.StartSpawn();
        bossEnemySpawner.StartSpawn();
    }
}
