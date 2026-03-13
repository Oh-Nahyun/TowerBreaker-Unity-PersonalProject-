using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlame : MonoBehaviour
{
    /// <summary>
    /// 불꽃 손 시작 이펙트용 프리팹
    /// </summary>
    public GameObject startEffectPrefab;

    /// <summary>
    /// 불꽃 손 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// 불꽃 손 범위
    /// </summary>
    public int range = 1;

    /// <summary>
    /// 적 검병
    /// </summary>
    SwordSoldier swordSoldier;

    /// <summary>
    /// 불꽃 손 공격력
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
        }
    }
}
