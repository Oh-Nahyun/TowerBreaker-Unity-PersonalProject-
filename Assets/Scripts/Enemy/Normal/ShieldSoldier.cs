using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldSoldier : Enemy
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 방패병 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 방패병 충돌 후 넉백 거리
    /// </summary>
    public float knockbackDistance = 0.1f;

    /// <summary>
    /// 방패병 이동 시간
    /// </summary>
    public float moveDuration = 1.0f;

    /// <summary>
    /// 방패병 방어 시간
    /// </summary>
    public float guardDuration = 1.0f;

    /// <summary>
    /// 방패병 이동 상태
    /// </summary>
    public bool isOnMove = false;

    /// <summary>
    /// 방패병 방어 상태
    /// </summary>
    public bool isOnGuard = false;

    /// <summary>
    /// 방패병 애니메이터용 해시값
    /// </summary>
    protected readonly int IsGuardHash = Animator.StringToHash("IsGuard");

    protected override void Start()
    {
        base.Start();

        //GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        //if (playerObject != null)
        //{
        //    player = playerObject.GetComponent<Player>();
        //}

        StartCoroutine(GuardCoroutine());
    }

    private void FixedUpdate()
    {
        if (!IsAlive() || !isOnMove || isOnGuard)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        Vector2 nextPosition = rigid2d.position + Vector2.left * moveSpeed * Time.fixedDeltaTime;
        rigid2d.MovePosition(nextPosition);
    }

    public override void TakeDamage(int damage)
    {
        if (isOnGuard)
        {
            damage = Mathf.RoundToInt(damage * 0.5f);
        }

        base.TakeDamage(damage);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeKnockback(knockbackDistance);
            }
        }
    }

    private IEnumerator GuardCoroutine()
    {
        while (IsAlive())
        {
            isOnMove = true;
            isOnGuard = false;

            animator.SetBool(IsGuardHash, isOnGuard);

            yield return new WaitForSeconds(moveDuration);

            isOnMove = false;
            isOnGuard = true;

            animator.SetBool(IsGuardHash, isOnGuard);

            yield return new WaitForSeconds(guardDuration);
        }
    }
}
