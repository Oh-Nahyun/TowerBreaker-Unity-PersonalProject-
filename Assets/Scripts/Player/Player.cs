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
    /// 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 0.1f;

    /// <summary>
    /// 이동 방향
    /// </summary>
    Vector3 inputDir = Vector3.zero;

    /// <summary>
    /// 애니메이터용 해시값
    /// </summary>
    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int DefenseHash = Animator.StringToHash("Defense");
    readonly int NormalAttackHash = Animator.StringToHash("NormalAttack");
    readonly int HardAttackHash = Animator.StringToHash("HardAttack");
    readonly int RangeAttackHash = Animator.StringToHash("RangeAttack");
    readonly int StabAttackHash = Animator.StringToHash("StabAttack");

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
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

        inputActions.Player.HardAttack.performed += OnHardAttackInput;
        inputActions.Player.HardAttack.canceled += OnHardAttackInput;

        inputActions.Player.RangeAttack.performed += OnRangeAttackInput;
        inputActions.Player.RangeAttack.canceled += OnRangeAttackInput;

        inputActions.Player.StabAttack.performed += OnStabAttackInput;
        inputActions.Player.StabAttack.canceled += OnStabAttackInput;
    }

    private void OnDisable()
    {
        inputActions.Player.StabAttack.canceled -= OnStabAttackInput;
        inputActions.Player.StabAttack.performed -= OnStabAttackInput;

        inputActions.Player.RangeAttack.canceled -= OnRangeAttackInput;
        inputActions.Player.RangeAttack.performed -= OnRangeAttackInput;

        inputActions.Player.HardAttack.canceled -= OnHardAttackInput;
        inputActions.Player.HardAttack.performed -= OnHardAttackInput;

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

    private void OnHardAttackInput(InputAction.CallbackContext context)
    {
        SetHardAttackInput(!context.canceled);
    }

    private void OnRangeAttackInput(InputAction.CallbackContext context)
    {
        SetRangeAttackInput(!context.canceled);
    }

    private void OnStabAttackInput(InputAction.CallbackContext context)
    {
        SetStabAttackInput(!context.canceled);
    }

    void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * inputDir, Space.World);
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

    void SetHardAttackInput(bool IsHardAttack)
    {
        if (IsHardAttack)
        {
            Debug.Log("강공격 스킬 시작");
            animator.SetTrigger(HardAttackHash);
        }
        else
        {
            Debug.Log("강공격 스킬 종료");
        }
    }

    private void SetRangeAttackInput(bool IsRangeAttack)
    {
        if (IsRangeAttack)
        {
            Debug.Log("범위 공격 스킬 시작");
            animator.SetTrigger(RangeAttackHash);
        }
        else
        {
            Debug.Log("범위 공격 스킬 종료");
        }
    }

    private void SetStabAttackInput(bool IsStabAttack)
    {
        if (IsStabAttack)
        {
            Debug.Log("찌르기 스킬 시작");
            animator.SetTrigger(StabAttackHash);
        }
        else
        {
            Debug.Log("찌르기 스킬 종료");
        }
    }
}
