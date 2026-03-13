using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSoldier : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x - Time.deltaTime * moveSpeed, 0.0f, 0.0f);
    }
}
