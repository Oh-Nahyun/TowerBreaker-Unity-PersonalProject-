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
    /// 플레이어 리지드바디 타입
    /// </summary>
    RigidbodyType2D rigid2dType;

    /// <summary>
    /// 플레이어 콜라이더
    /// </summary>
    Collider2D collider2d;

    /// <summary>
    /// 적 배열
    /// </summary>
    Enemy[] enemy;

    /// <summary>
    /// 플레이어 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 플레이어 이동 시간
    /// </summary>
    public float moveDuration = 0.1f;

    /// <summary>
    /// 플레이어 이동 상태
    /// </summary>
    private bool isOnMove = false;

    /// <summary>
    /// 플레이어 방어 후 이동 속도
    /// </summary>
    public float defenseSpeed = 5.0f;

    /// <summary>
    /// 플레이어 방어 시간
    /// </summary>
    public float defenseDuration = 0.1f;

    /// <summary>
    /// 방어 시 적이 움직일 거리
    /// </summary>
    public Vector3 movePosition = new Vector3(1.0f, 0.0f, 0.0f);

    /// <summary>
    /// 플레이어 방어 상태
    /// </summary>
    private bool isOnDefense = false;

    /// <summary>
    /// 플레이어 체력
    /// </summary>
    protected int health = 100;
    public int Health => health;

    /// <summary>
    /// 플레이어 하트 갯수
    /// </summary>
    [Range(1, 10)]
    public int heartCount = 3;

    /// <summary>
    /// 플레이어 사망 상태
    /// </summary>
    private bool isDead = false;

    /// <summary>
    /// 플레이어 공격 상태
    /// </summary>
    public bool isOnAttack = false;

    /// <summary>
    /// 플레이어 넉백 상태
    /// </summary>
    private bool isOnKnockback = false;

    /// <summary>
    /// 플레이어 넉백 시간
    /// </summary>
    private float knockbackDuration = 0.1f;

    /// <summary>
    /// 플레이어 일반 공격 사용 상태
    /// </summary>
    private bool isNormalAttacking = false;

    /// <summary>
    /// 플레이어 일반 공격 간격
    /// </summary>
    public float normalAttackIntercal = 0.5f;

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
    public GameObject defenseEffectPoint;

    /// <summary>
    /// 강공격 스킬 시작 위치
    /// </summary>
    public GameObject hardSkillPoint;

    /// <summary>
    /// 범위 스킬 시작 위치
    /// </summary>
    public GameObject rangeSkillPoint;

    /// <summary>
    /// 찌르기 스킬 시작 위치
    /// </summary>
    public GameObject stabSkillPoint;

    /// <summary>
    /// 플레이어 애니메이터용 해시값
    /// </summary>
    readonly int MoveHash = Animator.StringToHash("Move");
    readonly int DefenseHash = Animator.StringToHash("Defense");
    readonly int NormalAttackHash = Animator.StringToHash("NormalAttack");
    readonly int HardSkillHash = Animator.StringToHash("HardSkill");
    readonly int RangeSkillHash = Animator.StringToHash("RangeSkill");
    readonly int StabSkillHash = Animator.StringToHash("StabSkill");
    readonly int DeathHash = Animator.StringToHash("Death");
    readonly int IsDeathHash = Animator.StringToHash("IsDeath");

    /// <summary>
    /// 함수 저장용 변수
    /// </summary>
    public Action<int> onDie;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponentInChildren<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
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

    public void SetMoveInput(bool IsMove)
    {
        if (!IsAlive() || isOnAttack || isOnMove || isOnKnockback)
        {
            return;
        }

        if (IsMove)
        {
            StartCoroutine(MoveCoroutine());
        }
    }

    public void SetDefenseInput(bool IsDefense)
    {
        if (!IsAlive() || isOnAttack || isOnDefense || isOnKnockback)
        {
            return;
        }

        if (IsDefense)
        {
            StartCoroutine(DefenseCoroutine());
        }
    }

    public void SetNormalAttackInput(bool IsNormalAttack)
    {
        if (IsNormalAttack)
        {
            if (!isNormalAttacking)
            {
                isNormalAttacking = true;
                StartCoroutine(NormalAttackCoroutine());
            }
        }
        else
        {
            isNormalAttacking = false;
        }
    }

    public void SetHardSkillInput(bool IsHardSkill)
    {
        if (IsHardSkill)
        {
            animator.SetTrigger(HardSkillHash);
        }
    }

    public void SetRangeSkillInput(bool IsRangeSkill)
    {
        if (IsRangeSkill)
        {
            animator.SetTrigger(RangeSkillHash);
        }
    }

    public void SetStabSkillInput(bool IsStabSkill)
    {
        if (IsStabSkill)
        {
            animator.SetTrigger(StabSkillHash);
        }
    }

    private bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        onDie?.Invoke(0);

        animator.SetBool(IsDeathHash, !IsAlive());
        animator.SetTrigger(DeathHash);

        DisableCollider();
    }

    private void DisableCollider()
    {
        collider2d.enabled = false;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive() || isDead)
        {
            return;
        }

        health -= damage;
        health = Mathf.Clamp(health, 0, 100);

        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback(float knockbackDistance)
    {
        if (isOnKnockback)
        {
            return;
        }

        StartCoroutine(KnockbackCoroutine(Vector2.left, knockbackDistance, knockbackDuration));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isOnDefense || !collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        enemy = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        for (int i = 0; i < enemy.Length; i++)
        {
            enemy[i].transform.position += movePosition;
        }

        StartCoroutine(RunCoroutine(defenseSpeed, defenseDuration, Vector2.left));
    }

    private IEnumerator RunCoroutine(float runSpeed, float runDuration, Vector2 runDirection)
    {
        float currentTime = 0.0f;
        while (currentTime < runDuration)
        {
            currentTime += Time.fixedDeltaTime;

            Vector2 targetPosition = rigid2d.position + runDirection * runSpeed * Time.fixedDeltaTime;
            rigid2d.MovePosition(targetPosition);

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator MoveCoroutine()
    {
        isOnMove = true;
        animator.SetTrigger(MoveHash);
        yield return StartCoroutine(RunCoroutine(moveSpeed, moveDuration, Vector2.right));
        isOnMove = false;
    }

    private IEnumerator DefenseCoroutine()
    {
        isOnDefense = true;
        animator.SetTrigger(DefenseHash);
        yield return new WaitForSeconds(defenseDuration);
        isOnDefense = false;
    }

    private IEnumerator NormalAttackCoroutine()
    {
        while (isNormalAttacking)
        {
            animator.SetTrigger(NormalAttackHash);
            yield return new WaitForSeconds(normalAttackIntercal);
        }
    }

    private IEnumerator KnockbackCoroutine(Vector2 knockbackDirection, float knockbackDistance, float knockbackDuration)
    {
        isOnKnockback = true;
        ChangeRigidbodyTypeToKinematic();

        Vector2 startPosition = rigid2d.position;
        Vector2 targetPosition = startPosition + knockbackDirection * knockbackDistance;

        float currentTime = 0.0f;
        while (currentTime < knockbackDuration)
        {
            currentTime += Time.fixedDeltaTime;
            float time = currentTime / knockbackDuration;

            Vector2 nextPosition = Vector2.Lerp(startPosition, targetPosition, time);
            rigid2d.MovePosition(nextPosition);

            yield return new WaitForFixedUpdate();
        }

        rigid2d.MovePosition(targetPosition);
        ChangeRigidbodyTypeToOriginal();

        isOnKnockback = false;
    }

    public void ChangeRigidbodyTypeToKinematic()
    {
        rigid2dType = rigid2d.bodyType;
        rigid2d.bodyType = RigidbodyType2D.Kinematic;
        rigid2d.velocity = Vector2.zero;
        rigid2d.angularVelocity = 0.0f;
    }

    public void ChangeRigidbodyTypeToOriginal()
    {
        rigid2d.bodyType = rigid2dType;
        rigid2d.velocity = Vector2.zero;
        rigid2d.angularVelocity = 0.0f;
    }

    public void PlayDefenseEffect()
    {
        Instantiate(defenseEffectPrefab, defenseEffectPoint.transform.position, Quaternion.identity, defenseEffectPoint.transform);
    }

    public void FireWindGround()
    {
        Instantiate(windGroundPrefab, hardSkillPoint.transform.position, Quaternion.identity);
    }

    public void FireFlameHand()
    {
        Instantiate(flameHandPrefab, rangeSkillPoint.transform.position, Quaternion.identity);
    }

    public void FireProjectile()
    {
        Instantiate(projectilePrefab, stabSkillPoint.transform.position, Quaternion.identity);
    }
}
