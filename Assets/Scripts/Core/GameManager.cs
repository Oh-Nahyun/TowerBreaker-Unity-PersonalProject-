using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 게임 매니저
    /// </summary>
    public static GameManager Instance;

    /// <summary>
    /// 게임 배경 음악
    /// </summary>
    [SerializeField]
    AudioClip bgmClip;
    AudioSource bgmSource;

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

    /// <summary>
    /// 장비 도감
    /// </summary>
    public List<EquipmentData> allEquipments = new List<EquipmentData>();

    /// <summary>
    /// 스테이지 클리어 보상
    /// </summary>
    public List<StageClearReward> stageClearRewards = new List<StageClearReward>();

    /// <summary>
    /// 보유 장비
    /// </summary>
    public List<int> ownedEquipmentIds = new List<int>();

    /// <summary>
    /// 장착 장비 무기 아이디
    /// </summary>
    public int equippedWeaponId = -1;

    /// <summary>
    /// 장착 장비 방어구 아이디
    /// </summary>
    public int equippedArmorId = -1;
    

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

    private void Start()
    {
        OnInitialize();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = GetComponent<AudioSource>();
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
        }

        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = 1.0f;

        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        player = FindAnyObjectByType<Player>();
        InGamePanel = FindAnyObjectByType<InGamePanel>();

        if (InGamePanel != null)
        {
            InGamePanel.UpdateGameStageUI(currentStage);
            InGamePanel.SetBlockRaycast(true);

            normalEnemySpawner = FindAnyObjectByType<NormalEnemySpawner>();
            bossEnemySpawner = FindAnyObjectByType<BossEnemySpawner>();

            totalEnemyCount = normalEnemySpawner.enemySpawnCount + bossEnemySpawner.enemySpawnCount;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnInitialize();
    }

    public void CountDeadEnemy()
    {
        deadEnemyCount++;

        if (deadEnemyCount >= totalEnemyCount)
        {
            GiveClearReward(currentStage);

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

    /// -----------------
    /// 아래 장비 구현 진행중
    /// -----------------

    public EquipmentData GetEquipmentById(int id)
    {
        return allEquipments.Find(e => e.id == id);
    }

    public bool HasEquipment(int id)
    {
        return ownedEquipmentIds.Contains(id);
    }

    public void AddEquipment(int id)
    {
        if (HasEquipment(id)) return;

        ownedEquipmentIds.Add(id);

        EquipmentData equipment = GetEquipmentById(id);
        if (equipment != null)
            Debug.Log($"장비 획득 : {equipment.equipmentName}");
    }

    public void GiveClearReward(int stageId)
    {
        StageClearReward reward = stageClearRewards.Find(r => r.stageId == stageId);
        if (reward == null) return;

        AddEquipment(reward.equipmentId);
    }

    public void Equip(int id)
    {
        EquipmentData equipment = GetEquipmentById(id);
        if (equipment == null) return;
        if (!HasEquipment(id)) return;

        if (equipment.type == EquipmentType.Weapon)
            equippedWeaponId = id;
        else if (equipment.type == EquipmentType.Armor)
            equippedArmorId = id;

        ApplyEquipmentStats();
    }

    public void Unequip(EquipmentType type)
    {
        if (type == EquipmentType.Weapon)
            equippedWeaponId = -1;
        else if (type == EquipmentType.Armor)
            equippedArmorId = -1;

        ApplyEquipmentStats();
    }

    public void ApplyEquipmentStats()
    {
        int totalAttackBonus = 0;
        int totalDefenseBonus = 0;

        if (equippedWeaponId != -1)
        {
            EquipmentData weapon = GetEquipmentById(equippedWeaponId);
            if (weapon != null)
            {
                totalAttackBonus += weapon.attackBonus;
                totalDefenseBonus += weapon.defenseBonus;
            }
        }

        if (equippedArmorId != -1)
        {
            EquipmentData armor = GetEquipmentById(equippedArmorId);
            if (armor != null)
            {
                totalAttackBonus += armor.attackBonus;
                totalDefenseBonus += armor.defenseBonus;
            }
        }

        Player.SetEquipmentBonus(totalAttackBonus, totalDefenseBonus);
    }
}
