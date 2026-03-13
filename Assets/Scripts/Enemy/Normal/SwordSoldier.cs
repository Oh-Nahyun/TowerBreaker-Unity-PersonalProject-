using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSoldier : MonoBehaviour
{
    /// <summary>
    /// 검병 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    /// <summary>
    /// 검병 체력
    /// </summary>
    public int health = 100;
    public int Health
    {
        get => health;
        private set
        {
            if (health != value)
            {
                health = Mathf.Min(value, 100);
            }

            if (health <= 0)
            {
                health = 0;
            }
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime * moveSpeed, 0.0f, 0.0f);
    }

    public bool IsAlive()
    {
        return health > 0;
    }
}
