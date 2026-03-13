using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    public Player player;

    public void FireProjectile()
    {
        player.FireProjectile();
    }
}
