using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 화살 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 화살 수명
    /// </summary>
    public float lifeTime = 3.0f;

    /// <summary>
    /// 화살 발사 후 넉백 거리
    /// </summary>
    public float knockbackDistanceAfterFireArrow = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponentInParent<Player>();

        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeKnockback(knockbackDistanceAfterFireArrow);
            Destroy(gameObject);
        }
    }
}
