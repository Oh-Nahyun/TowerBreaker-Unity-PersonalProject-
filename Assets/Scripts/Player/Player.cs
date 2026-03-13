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
    readonly int AttackHash = Animator.StringToHash("Attack");
    readonly int DefenseHash = Animator.StringToHash("Defense");

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

        inputActions.Player.Attack.performed += OnAttackInput;
        inputActions.Player.Attack.canceled += OnAttackInput;

        inputActions.Player.Defense.performed += OnDefenseInput;
        inputActions.Player.Defense.canceled += OnDefenseInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Defense.canceled -= OnDefenseInput;
        inputActions.Player.Defense.performed -= OnDefenseInput;

        inputActions.Player.Attack.canceled -= OnAttackInput;
        inputActions.Player.Attack.performed -= OnAttackInput;

        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;

        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        SetMoveInput(!context.canceled);
    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {
        SetAttackInput(!context.canceled);
    }

    private void OnDefenseInput(InputAction.CallbackContext context)
    {
        SetDefenseInput(!context.canceled);
    }

    void SetMoveInput(bool IsMove)
    {
        animator.SetBool(IsMoveHash, IsMove);
    }

    void Move()
    {
        transform.Translate(Time.deltaTime * moveSpeed * inputDir, Space.World);
    }

    void SetAttackInput(bool IsAttack)
    {
        if (IsAttack)
        {
            Debug.Log("공격 시작");
            animator.SetTrigger(AttackHash);
        }
        else
        {
            Debug.Log("공격 종료");
        }
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
}
