using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSoldier : Enemy
{
    /// <summary>
    /// 플레이어
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 빛속성 보스 낙뢰 프리팹
    /// </summary>
    public GameObject lightningPrefab;

    /// <summary>
    /// 빛속성 보스 폭발 프리팹
    /// </summary>
    public GameObject explosionPrefab;

    /// <summary>
    /// 빛속성 보스 낙뢰 트랜스폼
    /// </summary>
    public Transform lightningPoint;

    /// <summary>
    /// 빛속성 보스 폭발 트랜스폼
    /// </summary>
    public Transform explosionPoint;

    /// <summary>
    /// 빛속성 보스 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 빛속성 보스 패턴 간 대기 시간
    /// </summary>
    public float patternCoolTime = 1.0f;

    /// <summary>
    /// 빛속성 보스 낙뢰 이후 폭발까지의 연계 시간
    /// </summary>
    public float lightningToExplosionTime = 0.5f;

    /// <summary>
    /// 빛속성 보스 낙뢰 시전 전 준비 시간
    /// </summary>
    public float lightningPrepareTime = 0.3f;

    /// <summary>
    /// 빛속성 보스 폭발 시전 전 준비 시간
    /// </summary>
    public float explosionPrepareTime = 0.3f;

    /// <summary>
    /// 빛속성 보스 낙뢰 랜덤 위치 범위
    /// </summary>
    public float lightningRandomRange = 2.0f;

    /// <summary>
    /// 빛속성 보스 연속 낙뢰 횟수
    /// </summary>
    public int lightningCount = 3;

    /// <summary>
    /// 빛속성 보스 연속 낙뢰 간격
    /// </summary>
    public float lightningInterval = 0.2f;

    /// <summary>
    /// 빛속성 보스 패턴 사용 상태
    /// </summary>
    private bool isOnPattern = false;

    /// <summary>
    /// 빛속성 보스 패턴 사용 범위 내 플레이어 존재 여부
    /// </summary>
    private bool isPlayerInTrigger = false;

    /// <summary>
    /// 빛속성 보스 애니메이터용 해시값
    /// </summary>
    protected readonly int IsMoveHash = Animator.StringToHash("IsMove");
    protected readonly int FireLightningHash = Animator.StringToHash("FireLightning");
    protected readonly int FireExplosionHash = Animator.StringToHash("FireExplosion");

    private void FixedUpdate()
    {
        if (!IsAlive())
        {
            return;
        }

        if (isOnPattern)
        {
            StopMove();
            return;
        }

        if (!isPlayerInTrigger)
        {
            Move();
            return;
        }

        StopMove();
        StartCoroutine(PatternCoroutine());
    }

    private void Move()
    {
        animator.SetBool(IsMoveHash, true);

        Vector2 nextPosition = rigid2d.position + Vector2.left * moveSpeed * Time.fixedDeltaTime;
        rigid2d.MovePosition(nextPosition);
    }

    private void StopMove()
    {
        animator.SetBool(IsMoveHash, false);
    }

    private float Lightning()
    {
        if (lightningPrefab != null && lightningPoint != null)
        {
            float offsetX = UnityEngine.Random.Range(-lightningRandomRange, lightningRandomRange);
            float spawnX = player.transform.position.x + offsetX;

            Vector3 spawnPosition = new Vector3(spawnX, lightningPoint.position.y, 0.0f);
            Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);

            return spawnX;
        }

        return 0.0f;
    }

    private void Explosion(float spawnX)
    {
        if (explosionPrefab != null && explosionPoint != null)
        {
            Vector3 spawnPosition = new Vector3(spawnX, explosionPoint.position.y, 0.0f);
            Instantiate(explosionPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
        }
    }

    private IEnumerator PatternCoroutine()
    {
        isOnPattern = true;

        int patternCase = UnityEngine.Random.Range(0, 3);
        switch (patternCase)
        {
            case 0:
                yield return StartCoroutine(SingleLightningPatternCoroutine());
                break;
            case 1:
                yield return StartCoroutine(SeveralLightningPatternCoroutine());
                break;
            case 2:
                yield return StartCoroutine(LightningAndExplosionPatternCoroutine());
                break;
        }

        yield return new WaitForSeconds(patternCoolTime);

        isOnPattern = false;
    }

    private IEnumerator SingleLightningPatternCoroutine()
    {
        animator.SetTrigger(FireLightningHash);

        yield return new WaitForSeconds(lightningPrepareTime);

        Lightning();
    }

    private IEnumerator SeveralLightningPatternCoroutine()
    {
        for (int i = 0; i < lightningCount; i++)
        {
            animator.SetTrigger(FireLightningHash);

            yield return new WaitForSeconds(lightningPrepareTime);

            Lightning();

            yield return new WaitForSeconds(lightningInterval);
        }
    }

    private IEnumerator LightningAndExplosionPatternCoroutine()
    {
        animator.SetTrigger(FireLightningHash);

        yield return new WaitForSeconds(lightningPrepareTime);

        float targetX = Lightning();

        yield return new WaitForSeconds(lightningToExplosionTime);

        animator.SetTrigger(FireExplosionHash);

        yield return new WaitForSeconds(explosionPrepareTime);

        Explosion(targetX);
    }

    public override void Die()
    {
        StopAllCoroutines();
        isOnPattern = false;
        isPlayerInTrigger = false;
        StopMove();

        base.Die();
    }
}
