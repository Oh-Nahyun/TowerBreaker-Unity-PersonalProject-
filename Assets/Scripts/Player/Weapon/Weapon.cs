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
    /// 무기 공격력
    /// </summary>
    public int attackPower = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!player.isOnAttack)
        {
            return;
        }

        Enemy enemy = collision.GetComponentInParent<Enemy>();

        if (enemy != null)
        {
            enemy.TakeDamage(attackPower);
        }
    }
}
