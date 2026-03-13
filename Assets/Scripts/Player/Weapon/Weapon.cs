using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player player;

    /// <summary>
    /// 적 검병
    /// </summary>
    SwordSoldier swordSoldier;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int attackPower = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isOnAttack)
        {
            return;
        }

        swordSoldier = collision.GetComponent<SwordSoldier>();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            swordSoldier.health -= attackPower;
            if (!swordSoldier.IsAlive())
            {
                swordSoldier.Die();
            }
        }
    }
}
