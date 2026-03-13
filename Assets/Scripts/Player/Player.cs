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
    /// 플레이어 이동 속도
    /// </summary>
    public float moveSpeed = 0.1f;

    /// <summary>
    /// 플레이어 이동 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 플레이어 애니메이터용 해시값
    /// </summary>
    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int DefenseHash = Animator.StringToHash("Defense");
    readonly int NormalAttackHash = Animator.StringToHash("NormalAttack");
    readonly int HardSkillHash = Animator.StringToHash("HardSkill");
    readonly int RangeSkillHash = Animator.StringToHash("RangeSkill");
    readonly int StabSkillHash = Animator.StringToHash("StabSkill");

    /// <summary>
    /// 스킬 발사체 프리팹
    /// </summary>
    public GameObject projectilePrefab;

    /// <summary>
    /// 스킬 발사체 발사 위치
    /// </summary>
    Transform projectileTransform;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        animator = GetComponentInChildren<Animator>();

        projectileTransform = transform.GetChild(1);
    }

    private void FixedUpdate()
    {
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
        transform.Translate(Time.deltaTime * moveSpeed * inputDir, Space.World);
    }

    public void FireProjectile()
    {
        Instantiate(projectilePrefab, projectileTransform.position, Quaternion.identity);
    }
}
