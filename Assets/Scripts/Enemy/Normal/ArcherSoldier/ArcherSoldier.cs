using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSoldier : Enemy
{
    /// <summary>
    /// 활병 화살 프리팹
    /// </summary>
    public GameObject arrowPrefab;

    /// <summary>
    /// 활병 화살 발사 위치
    /// </summary>
    public GameObject fireArrowPoint;

    /// <summary>
    /// 활병 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 활병 이동 시간
    /// </summary>
    public float moveDuration = 1.0f;

    /// <summary>
    /// 활병 화살 발사 시간
    /// </summary>
    public float fireArrowDuration = 1.0f;

    /// <summary>
    /// 활병 이동 상태
    /// </summary>
    public bool isOnMove = false;

    /// <summary>
    /// 활병 화살 발사 상태
    /// </summary>
    public bool isOnFireArrow = false;

    /// <summary>
    /// 활병 애니메이터용 해시값
    /// </summary>
    protected readonly int FireArrowHash = Animator.StringToHash("FireArrow");

    protected override void Start()
    {
        base.Start();

        StartCoroutine(FireArrowCoroutine());
    }

    private void FixedUpdate()
    {
        if (!IsAlive() || !isOnMove || isOnFireArrow)
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

    public void FireArrow()
    {
        Instantiate(arrowPrefab, fireArrowPoint.transform.position, Quaternion.identity);
    }

    private IEnumerator FireArrowCoroutine()
    {
        while (IsAlive())
        {
            isOnMove = true;
            isOnFireArrow = false;

            animator.SetBool(FireArrowHash, isOnFireArrow);

            yield return new WaitForSeconds(moveDuration);

            isOnMove = false;
            isOnFireArrow = true;

            animator.SetBool(FireArrowHash, isOnFireArrow);

            yield return new WaitForSeconds(fireArrowDuration);
        }
    }
}
