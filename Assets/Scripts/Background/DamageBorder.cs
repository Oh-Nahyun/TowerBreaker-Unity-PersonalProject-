using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBorder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponentInParent<Player>();

        if (player != null)
        {
            int damage = 100 / player.heartCount; 
            player.TakeDamage(damage);
            Debug.Log(player.Health);
        }
    }
}
