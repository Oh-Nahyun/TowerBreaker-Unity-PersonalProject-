using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    /// <summary>
    /// 적
    /// </summary>
    public ArcherSoldier archerSoldier;

    public void FireArrow()
    {
        archerSoldier.FireArrow();
    }
}
