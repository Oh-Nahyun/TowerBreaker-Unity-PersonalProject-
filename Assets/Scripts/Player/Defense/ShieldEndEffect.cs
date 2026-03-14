using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEndEffect : MonoBehaviour
{
    /// <summary>
    /// 방어 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 적 검병 배열
    /// </summary>
    SwordSoldier[] swordSoldiers;

    /// <summary>
    /// 방어 애니메이션 길이
    /// </summary>
    float endAnimLength = 0.0f;
    public float EndAnimLength => endAnimLength;

    /// <summary>
    /// 방어 시 움직일 거리
    /// </summary>
    public Vector3 movePosition = new Vector3(1.0f, 0.0f, 0.0f);

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        endAnimLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * 5;
    }

    private void Start()
    {
        Destroy(gameObject, endAnimLength);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Enemy"))
        {
            return;
        }

        Debug.Log("방어 범위 안으로 적 들어옴!");
        swordSoldiers = FindObjectsByType<SwordSoldier>(FindObjectsSortMode.None);

        for (int i = 0; i < swordSoldiers.Length; i++)
        {
            swordSoldiers[i].transform.position += movePosition;
        }
    }
}
