using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SwordSoldier : MonoBehaviour
{
    /// <summary>
    /// 검병 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 검병 콜라이더
    /// </summary>
    Collider2D collider2d;

    /// <summary>
    /// 검병 스프라이트들
    /// </summary>
    SpriteRenderer[] spriteRenderers;

    /// <summary>
    /// 검병 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 검병 체력
    /// </summary>
    public int health = 100;
    public int Health
    {
        get => health;
        private set
        {
            if (health != value)
            {
                health = Mathf.Min(value, 100);
            }

            if (health <= 0)
            {
                health = 0;
            }
        }
    }

    /// <summary>
    /// 검병 애니메이터용 해시값
    /// </summary>
    readonly int IsDeathHash = Animator.StringToHash("IsDeath");
    readonly int DeathHash = Animator.StringToHash("Death");

    /// <summary>
    /// 검병 사망 시 알파값
    /// </summary>
    float deathColorAlpha = 0.0f;

    /// <summary>
    /// 검병 사망 시 알파값 변경 시간
    /// </summary>
    float changeColorDuration = 1.0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        collider2d = GetComponentInChildren<Collider2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        animator.SetBool(IsDeathHash, IsAlive());
    }

    private void Update()
    {
        if (IsAlive())
        {
            transform.position = new Vector3(transform.position.x - Time.deltaTime * moveSpeed, 0.0f, 0.0f);
        }
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    public void Die()
    {
        Debug.Log("검병 사망");

        animator.SetBool(IsDeathHash, !IsAlive());
        animator.SetTrigger(DeathHash);

        DisableCollider();
        StartCoroutine(DieProcessCoroutine());
    }

    private void DisableCollider()
    {
        collider2d.enabled = false;
    }

    private IEnumerator DieProcessCoroutine()
    {
        yield return StartCoroutine(ChangeColorCoroutine());
        Destroy(gameObject);
    }

    private IEnumerator ChangeColorCoroutine()
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
