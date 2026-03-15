using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// 파도 이동 속도
    /// </summary>
    public float moveSpeed = 7.0f;

    /// <summary>
    /// 파도 수명
    /// </summary>
    public float lifeTime = 10.0f;

    /// <summary>
    /// 파도 발사 후 넉백 거리
    /// </summary>
    public float knockbackDistanceAfterWaterWave = 0.1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector2.left, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();

        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeKnockback(knockbackDistanceAfterWaterWave);
        }
    }
}
