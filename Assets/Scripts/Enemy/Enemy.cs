using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// 적 애니메이터
    /// </summary>
    protected Animator animator;

    /// <summary>
    /// 적 리지드바디
    /// </summary>
    protected Rigidbody2D rigid2d;

    /// <summary>
    /// 적 콜라이더
    /// </summary>
    protected Collider2D collider2d;

    /// <summary>
    /// 적 스프라이트들
    /// </summary>
    protected SpriteRenderer[] spriteRenderers;

    /// <summary>
    /// 플레이어 트랜스폼
    /// </summary>
    protected Transform playerTransform;

    /// <summary>
    /// 적 체력
    /// </summary>
    protected int health = 100;
    public int Health => health;

    /// <summary>
    /// 적 사망 시 알파값
    /// </summary>
    public float deathColorAlpha = 0.0f;

    /// <summary>
    /// 적 사망 시 알파값 변경 시간
    /// </summary>
    protected float changeColorDuration = 1.0f;

    /// <summary>
    /// 적 애니메이터용 해시값
    /// </summary>
    protected readonly int IsDeathHash = Animator.StringToHash("IsDeath");
    protected readonly int DeathHash = Animator.StringToHash("Death");

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigid2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        animator.SetBool(IsDeathHash, IsAlive());
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive())
        {
            return;
        }

        health -= damage;
        health = Mathf.Clamp(health, 0, 100);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        animator.SetBool(IsDeathHash, !IsAlive());
        animator.SetTrigger(DeathHash);

        DisableCollider();
        StartCoroutine(DieProcessCoroutine());
    }

    protected void DisableCollider()
    {
        if (collider2d != null)
        {
            collider2d.enabled = false;
        }
    }

    protected IEnumerator DieProcessCoroutine()
    {
        yield return StartCoroutine(ChangeColorCoroutine());
        Destroy(gameObject);
    }

    protected IEnumerator ChangeColorCoroutine()
    {
        Color[] spriteColors = new Color[spriteRenderers.Length];
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteColors[i] = spriteRenderers[i].color;
        }

        float currentTime = 0.0f;
        while (currentTime < changeColorDuration)
        {
            currentTime += Time.deltaTime;
            float time = currentTime / changeColorDuration;

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Color color = spriteRenderers[i].color;
                color.a = Mathf.Lerp(spriteColors[i].a, deathColorAlpha, time);
                spriteRenderers[i].color = color;
            }

            yield return null;
        }

        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Color color = spriteRenderers[i].color;
            color.a = deathColorAlpha;
            spriteRenderers[i].color = color;
        }
    }
}
