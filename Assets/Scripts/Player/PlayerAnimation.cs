using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player player;

    public void FireFlameHand()
    {
        player.FireFlameHand();
    }

    public void FireProjectile()
    {
        player.FireProjectile();
    }

    public void ChangeAttackMode()
    {
        if (player.isOnAttack)
        {
            player.isOnAttack = false;
        }
        else
        {
            player.isOnAttack = true;
        }
    }
}
