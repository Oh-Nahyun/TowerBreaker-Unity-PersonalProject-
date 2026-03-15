using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 낙뢰 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// 낙뢰 후 넉백 거리
    /// </summary>
    public float knockbackDistanceAfterFireLightning = 0.1f;

    /// <summary>
    /// 플레이어 중복 피격 상태
    /// </summary>
    private bool hasHitPlayer = false;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitPlayer)
        {
            return;
        }

        player = collision.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;
            player.TakeKnockback(knockbackDistanceAfterFireLightning);
        }
    }
}
