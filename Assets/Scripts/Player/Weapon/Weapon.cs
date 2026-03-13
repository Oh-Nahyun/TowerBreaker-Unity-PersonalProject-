using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    /// <summary>
    /// 적 검병
    /// </summary>
    SwordSoldier swordSoldier;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        swordSoldier = FindAnyObjectByType<SwordSoldier>();

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (swordSoldier.health > 0)
            {
                swordSoldier.health -= 100;
            }
            else if (swordSoldier.health == 0)
            {
                Destroy(collision.gameObject);

            }
            else
            {
                swordSoldier.health = 0;
            }
        }
    }
}
