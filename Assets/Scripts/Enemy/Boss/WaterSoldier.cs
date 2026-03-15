using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSoldier : Enemy
{
    /// <summary>
    /// 플레이어
    /// </summary>
    public GameObject player;

    /// <summary>
    /// 물속성 보스 파도 프리팹
    /// </summary>
    public GameObject waterWavePrefab;

    /// <summary>
    /// 물속성 보스 솟구침 프리팹
    /// </summary>
    public GameObject holyRisePrefab;

    /// <summary>
    /// 물속성 보스 파도 트랜스폼
    /// </summary>
    public Transform waterWavePoint;

    /// <summary>
    /// 물속성 보스 솟구침 트랜스폼
    /// </summary>
    public Transform holyRisePoint;

    /// <summary>
    /// 물속성 보스 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 물속성 보스 패턴 간 대기 시간
    /// </summary>
    public float patternCoolTime = 1.0f;

    /// <summary>
    /// 물속성 보스 파도 이후 솟구침까지의 연계 시간
    /// </summary>
    public float waterWaveToHolyRiseTime = 0.5f;

    /// <summary>
    /// 물속성 보스 파도 시전 전 준비 시간
    /// </summary>
    public float waterWavePrepareTime = 0.3f;

    /// <summary>
    /// 물속성 보스 솟구침 시전 전 준비 시간
    /// </summary>
    public float holyRisePrepareTime = 0.3f;

    /// <summary>
    /// 물속성 보스 솟구침 랜덤 위치 범위
    /// </summary>
    public float holyRiseRandomRange = 1.0f;

    /// <summary>
    /// 물속성 보스 패턴 사용 상태
    /// </summary>
    private bool isOnPattern = false;

    /// <summary>
    /// 물속성 보스 패턴 사용 범위 내 플레이어 존재 여부
    /// </summary>
    private bool isPlayerInTrigger = false;

    /// <summary>
    /// 물속성 보스 애니메이터용 해시값
    /// </summary>
    protected readonly int IsMoveHash = Animator.StringToHash("IsMove");
    protected readonly int WaveHash = Animator.StringToHash("Wave");
    protected readonly int RiseHash = Animator.StringToHash("Rise");

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

    private void WaterWave()
    {
        if (waterWavePrefab != null && waterWavePoint != null)
        {
            Instantiate(waterWavePrefab, waterWavePoint.position, Quaternion.identity);
        }
    }

    private void HolyRise()
    {
        if (holyRisePrefab != null && holyRisePoint != null)
        {
            float offsetX = Random.Range(-holyRiseRandomRange, holyRiseRandomRange);
            float spawnX = player.transform.position.x + offsetX;

            Vector3 spawnPosition = new Vector3(spawnX, holyRisePoint.position.y, 0.0f);
            Instantiate(holyRisePrefab, spawnPosition, Quaternion.identity);
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

        int patternCase = Random.Range(0, 3);
        switch (patternCase)
        {
            case 0:
                yield return StartCoroutine(WavePatternCoroutine());
                break;
            case 1:
                yield return StartCoroutine(RisePatternCoroutine());
                break;
            case 2:
                yield return StartCoroutine(WaveAndRisePatternCoroutine());
                break;
        }

        yield return new WaitForSeconds(patternCoolTime);

        isOnPattern = false;
    }

    private IEnumerator WavePatternCoroutine()
    {
        animator.SetTrigger(WaveHash);

        yield return new WaitForSeconds(waterWavePrepareTime);

        WaterWave();
    }

    private IEnumerator RisePatternCoroutine()
    {
        animator.SetTrigger(RiseHash);

        yield return new WaitForSeconds(holyRisePrepareTime);

        HolyRise();
    }

    private IEnumerator WaveAndRisePatternCoroutine()
    {
        animator.SetTrigger(WaveHash);

        yield return new WaitForSeconds(waterWavePrepareTime);

        WaterWave();

        yield return new WaitForSeconds(waterWaveToHolyRiseTime);

        animator.SetTrigger(RiseHash);

        yield return new WaitForSeconds(holyRisePrepareTime);

        HolyRise();
    }
}
