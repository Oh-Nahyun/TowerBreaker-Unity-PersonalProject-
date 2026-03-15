using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlameEndEffect : MonoBehaviour
{
    /// <summary>
    /// 불꽃 손 종료 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 불꽃 손 종료 애니메이션 길이
    /// </summary>
    float endAnimLength = 0.0f;
    public float EndAnimLength => endAnimLength;

    /// <summary>
    /// 불꽃 손 공격력
    /// </summary>
    public int attackPower = 100;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        endAnimLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void Start()
    {
        Destroy(gameObject, endAnimLength);
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
