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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isOnAttack)
        {
            return;
        }

        swordSoldier = FindAnyObjectByType<SwordSoldier>();

        if (collision.gameObject.CompareTag("Enemy") && swordSoldier.IsAlive())
        {
            swordSoldier.health = 0;
            Destroy(collision.gameObject);
        }
    }
}
