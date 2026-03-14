using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileStartEffect : MonoBehaviour
{
    /// <summary>
    /// 발사체 발사 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 발사체 발사 애니메이션 길이
    /// </summary>
    float startAnimLength = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startAnimLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void Start()
    {
        Destroy(gameObject, startAnimLength);
    }
}
