using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Player : MonoBehaviour
{
    /// <summary>
    /// 플레이어 인풋 액션
    /// </summary>
    PlayerInputActions inputActions;

    /// <summary>
    /// 플레이어 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 플레이어 리지드바디
    /// </summary>
    Rigidbody2D rigid2d;

    /// <summary>
    /// 플레이어 콜라이더
    /// </summary>
    Collider2D collider2d;

    /// <summary>
    /// 플레이어 이동 방향
    /// </summary>
    Vector2 inputDir = Vector2.zero;

    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float moveSpeed = 0.1f;

    /// <summary>
    /// 플레이어 체력
    /// </summary>
    public int health = 100;
    public int Health
    {
        get => health;
        private set
        {
            if (health != value)
            {
                health = Mathf.Min(value, 100);
            }

            if (health <= 0)
            {
                health = 0;
            }
        }
    }

    /// <summary>
    /// 플레이어 공격 상태
    /// </summary>
    public bool isOnAttack = false;

    /// <summary>
    /// 플레이어 방어 효과 프리팹
    /// </summary>
    public GameObject defenseEffectPrefab;

    /// <summary>
    /// 스킬 바람 프리팹
    /// </summary>
    public GameObject windGroundPrefab;

    /// <summary>
    /// 스킬 불꽃 손 프리팹
    /// </summary>
    public GameObject flameHandPrefab;

    /// <summary>
    /// 스킬 발사체 프리팹
    /// </summary>
    public GameObject projectilePrefab;

    /// <summary>
    /// 방어 효과 시작 위치
    /// </summary>
    public GameObject defenseEffectSpawnPoint;

    /// <summary>
    /// 강공격 스킬 시작 위치
    /// </summary>
    public GameObject hardSkillSpawnPoint;

    /// <summary>
    /// 범위 스킬 시작 위치
    /// </summary>
    public GameObject rangeSkillSpawnPoint;

    /// <summary>
    /// 찌르기 스킬 시작 위치
    /// </summary>
    public GameObject stabSkillSpawnPoint;

    /// <summary>
    /// 플레이어 애니메이터용 해시값
    /// </summary>
    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int DefenseHash = Animator.StringToHash("Defense");
    readonly int NormalAttackHash = Animator.StringToHash("NormalAttack");
    readonly int HardSkillHash = Animator.StringToHash("HardSkill");
    readonly int RangeSkillHash = Animator.StringToHash("RangeSkill");
    readonly int StabSkillHash = Animator.StringToHash("StabSkill");
    readonly int DeathHash = Animator.StringToHash("Death");
    readonly int IsDeathHash = Animator.StringToHash("IsDeath");

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponentInChildren<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (!IsAlive() || isOnAttack)
        {
            return;
        }

        Move();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;

        inputActions.Player.Defense.performed += OnDefenseInput;
        inputActions.Player.Defense.canceled += OnDefenseInput;

        inputActions.Player.NormalAttack.performed += OnNormalAttackInput;
        inputActions.Player.NormalAttack.canceled += OnNormalAttackInput;

        inputActions.Player.HardSkill.performed += OnHardSkillInput;
        inputActions.Player.HardSkill.canceled += OnHardSkillInput;

        inputActions.Player.RangeSkill.performed += OnRangeSkillInput;
        inputActions.Player.RangeSkill.canceled += OnRangeSkillInput;

        inputActions.Player.StabSkill.performed += OnStabSkillInput;
        inputActions.Player.StabSkill.canceled += OnStabSkillInput;
    }

    private void OnDisable()
    {
        inputActions.Player.StabSkill.canceled -= OnStabSkillInput;
        inputActions.Player.StabSkill.performed -= OnStabSkillInput;

        inputActions.Player.RangeSkill.canceled -= OnRangeSkillInput;
        inputActions.Player.RangeSkill.performed -= OnRangeSkillInput;

        inputActions.Player.HardSkill.canceled -= OnHardSkillInput;
        inputActions.Player.HardSkill.performed -= OnHardSkillInput;

        inputActions.Player.NormalAttack.canceled -= OnNormalAttackInput;
        inputActions.Player.NormalAttack.performed -= OnNormalAttackInput;

        inputActions.Player.Defense.canceled -= OnDefenseInput;
        inputActions.Player.Defense.performed -= OnDefenseInput;

        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;

        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        SetMoveInput(!context.canceled);
    }

    private void OnDefenseInput(InputAction.CallbackContext context)
    {
        SetDefenseInput(!context.canceled);
    }

    private void OnNormalAttackInput(InputAction.CallbackContext context)
    {
        SetNormalAttackInput(!context.canceled);
    }

    private void OnHardSkillInput(InputAction.CallbackContext context)
    {
        SetHardSkillInput(!context.canceled);
    }

    private void OnRangeSkillInput(InputAction.CallbackContext context)
    {
        SetRangeSkillInput(!context.canceled);
    }

    private void OnStabSkillInput(InputAction.CallbackContext context)
    {
        SetStabSkillInput(!context.canceled);
    }

    void SetMoveInput(bool IsMove)
    {
        animator.SetBool(IsMoveHash, IsMove);
    }

    void SetDefenseInput(bool IsDefense)
    {
        if (IsDefense)
        {
            Debug.Log("방어 시작");
            animator.SetTrigger(DefenseHash);
        }
        else
        {
            Debug.Log("방어 종료");
        }
    }

    void SetNormalAttackInput(bool IsNormalAttack)
    {
        if (IsNormalAttack)
        {
            Debug.Log("일반 공격 시작");
            animator.SetTrigger(NormalAttackHash);
        }
        else
        {
            Debug.Log("일반 공격 종료");
        }
    }

    void SetHardSkillInput(bool IsHardSkill)
    {
        if (IsHardSkill)
        {
            Debug.Log("강공격 스킬 시작");
            animator.SetTrigger(HardSkillHash);
        }
        else
        {
            Debug.Log("강공격 스킬 종료");
        }
    }

    private void SetRangeSkillInput(bool IsRangeSkill)
    {
        if (IsRangeSkill)
        {
            Debug.Log("범위 스킬 시작");
            animator.SetTrigger(RangeSkillHash);
        }
        else
        {
            Debug.Log("범위 스킬 종료");
        }
    }

    private void SetStabSkillInput(bool IsStabSkill)
    {
        if (IsStabSkill)
        {
            Debug.Log("찌르기 스킬 시작");
            animator.SetTrigger(StabSkillHash);
        }
        else
        {
            Debug.Log("찌르기 스킬 종료");
        }
    }

    private void Move()
    {
        Vector2 newPosition = rigid2d.position + Time.fixedDeltaTime * moveSpeed * inputDir;
        rigid2d.MovePosition(newPosition);
    }

    private bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        Debug.Log("플레이어 사망");

        animator.SetBool(IsDeathHash, !IsAlive());
        animator.SetTrigger(DeathHash);

        DisableCollider();
    }

    private void DisableCollider()
    {
        collider2d.enabled = false;
    }

    public void PlayDefenseEffect()
    {
        Instantiate(defenseEffectPrefab, defenseEffectSpawnPoint.transform.position, Quaternion.identity, defenseEffectSpawnPoint.transform);
    }

    public void FireWindGround()
    {
        Instantiate(windGroundPrefab, hardSkillSpawnPoint.transform.position, Quaternion.identity);
    }

    public void FireFlameHand()
    {
        Instantiate(flameHandPrefab, rangeSkillSpawnPoint.transform.position, Quaternion.identity);
    }

    public void FireProjectile()
    {
        Instantiate(projectilePrefab, stabSkillSpawnPoint.transform.position, Quaternion.identity);
    }
}
