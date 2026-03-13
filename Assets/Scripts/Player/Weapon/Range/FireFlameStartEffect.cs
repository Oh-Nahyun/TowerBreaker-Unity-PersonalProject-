using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlameStartEffect : MonoBehaviour
{
    /// <summary>
    /// 불꽃 손 시작 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// 불꽃 손 시작 애니메이션 길이
    /// </summary>
    public float startAnimLength = 0.0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        startAnimLength = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
    }

    private void Start()
    {
        Destroy(this.gameObject, startAnimLength * 2);
    }
}
