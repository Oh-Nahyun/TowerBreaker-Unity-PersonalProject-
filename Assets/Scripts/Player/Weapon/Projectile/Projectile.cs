using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    /// <summary>
    /// 발사체 이동 속도
    /// </summary>
    public float moveSpeed = 5.0f;

    /// <summary>
    /// 발사체 발사 이펙트용 프리팹
    /// </summary>
    public GameObject startEffectPrefab;

    /// <summary>
    /// 발사체 종료 이펙트용 프리팹
    /// </summary>
    public GameObject endEffectPrefab;

    /// <summary>
    /// 발사체 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// 적 검병
    /// </summary>
    SwordSoldier swordSoldier;

    /// <summary>
    /// 발사체 공격력
    /// </summary>
    public int attackPower = 100;

    private void Start()
    {
        if (startEffectPrefab != null)
        {
            Instantiate(startEffectPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.right, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        swordSoldier = collision.GetComponent<SwordSoldier>();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            swordSoldier.health -= attackPower;

            if (!swordSoldier.IsAlive())
            {
                swordSoldier.Die();
            }

            Destroy(gameObject);

            if (endEffectPrefab != null)
            {
                GameObject endEffect = Instantiate(endEffectPrefab, transform.position, Quaternion.identity);
                Destroy(endEffect, 0.5f);
            }
        }
    }
}
