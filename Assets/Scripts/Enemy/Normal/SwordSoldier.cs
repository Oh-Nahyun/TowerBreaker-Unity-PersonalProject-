using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SwordSoldier : Enemy
{
    /// <summary>
    /// 검병 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 검병 돌진 속도
    /// </summary>
    public float dashSpeed = 2.0f;

    /// <summary>
    /// 검병 돌진 후 넉백 거리
    /// </summary>
    public float knockbackDistanceafterdash = 0.1f;

    /// <summary>
    /// 검병 돌진 시간
    /// </summary>
    public float dashDuration = 1.0f;

    /// <summary>
    /// 검병 돌진 전 준비 시간
    /// </summary>
    public float dashBeforePrepareTime = 0.25f;

    /// <summary>
    /// 검병 돌진 쿨타임
    /// </summary>
    public float dashCoolTime = 3.0f;

    /// <summary>
    /// 검병 돌진 상태
    /// </summary>
    private bool isOnDash = false;

    /// <summary>
    /// 검병 돌진 쿨타임 상태
    /// </summary>
    private bool isInDashCoolTime = false;

    /// <summary>
    /// 검병 돌진 시 플레이어 충돌 상태
    /// </summary>
    private bool hasHitPlayer = false;

    /// <summary>
    /// 검병 애니메이터용 해시값
    /// </summary>
    protected readonly int IsDashHash = Animator.StringToHash("IsDash");

    private void FixedUpdate()
    {
        if (!IsAlive() || isOnDash)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isOnDash || hasHitPlayer)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player)
            {
                hasHitPlayer = true;
                player.TakeKnockback(knockbackDistanceafterdash);
            }

            isOnDash = false;
            animator.SetBool(IsDashHash, isOnDash);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsAlive() || isOnDash || isInDashCoolTime)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isOnDash = true;
        hasHitPlayer = false;
        animator.SetBool(IsDashHash, isOnDash);

        yield return new WaitForSeconds(dashBeforePrepareTime);

        float currentTime = 0.0f;
        while (currentTime < dashDuration && isOnDash)
        {
            currentTime += Time.fixedDeltaTime;

            Vector2 targetPosition = rigid2d.position + Vector2.left * dashSpeed * Time.fixedDeltaTime;
            rigid2d.MovePosition(targetPosition);

            yield return new WaitForFixedUpdate();
        }

        isOnDash = false;
        animator.SetBool(IsDashHash, isOnDash);

        isInDashCoolTime = true;

        yield return new WaitForSeconds(dashCoolTime);

        isInDashCoolTime = false;
    }
}
