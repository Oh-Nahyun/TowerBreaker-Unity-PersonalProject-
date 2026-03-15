using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindGround : MonoBehaviour
{
    /// <summary>
    /// 바람 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 바람 애니메이션 길이
    /// </summary>
    float animLength = 0.0f;
    public float AnimLength => animLength;

    /// <summary>
    /// 바람 이동 속도
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// 바람 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// 바람 공격력
    /// </summary>
    public int attackPower = 100;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(attackPower);
        }
    }
}
