using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEndEffect : MonoBehaviour
{
    /// <summary>
    /// 발사체 종료 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 발사체 종료 애니메이션 길이
    /// </summary>
    float endAnimLength = 0.0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        endAnimLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void Start()
    {
        Destroy(this.gameObject, endAnimLength);
    }
}
