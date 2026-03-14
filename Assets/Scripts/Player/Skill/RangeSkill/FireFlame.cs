using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFlame : MonoBehaviour
{
    /// <summary>
    /// 불꽃 손 시작 이펙트용 프리팹
    /// </summary>
    public GameObject startEffectPrefab;

    /// <summary>
    /// 불꽃 손 종료 이펙트용 프리팹
    /// </summary>
    public GameObject endEffectPrefab;

    /// <summary>
    /// 불꽃 손 범위
    /// </summary>
    [Range(1, 3)]
    public int range = 1;

    private void Start()
    {
        StartCoroutine(FireFlameProcessCoroutine());
    }

    private IEnumerator FireFlameProcessCoroutine()
    {
        float startDelay = 0.0f;
        float endDelay = 0.0f;

        if (startEffectPrefab != null)
        {
            GameObject startEffect = Instantiate(startEffectPrefab, transform.position, Quaternion.identity);
            FireFlameStartEffect rangeSkillStartEffect = startEffect.GetComponent<FireFlameStartEffect>();
            startDelay = rangeSkillStartEffect.StartAnimLength * 2;
        }

        yield return new WaitForSeconds(startDelay);

        if (endEffectPrefab != null)
        {
            GameObject centerEndEffect = Instantiate(endEffectPrefab, transform.position, Quaternion.identity);
            FireFlameEndEffect rangeSkillEndEffect = centerEndEffect.GetComponent<FireFlameEndEffect>();
            endDelay = rangeSkillEndEffect.EndAnimLength * 0.5f;

            yield return new WaitForSeconds(endDelay);

            for (int i = 1; i <= range; i++)
            {
                Vector3 movePosition = new Vector3(i, 0.0f, 0.0f);

                Instantiate(endEffectPrefab, transform.position + movePosition, Quaternion.identity);
                Instantiate(endEffectPrefab, transform.position - movePosition, Quaternion.identity);

                yield return new WaitForSeconds(endDelay);
            }
        }

        Destroy(gameObject);
    }
}
