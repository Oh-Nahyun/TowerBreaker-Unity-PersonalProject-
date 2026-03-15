using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armor
}

[Serializable]
public class EquipmentData
{
    public int id;
    public string equipmentName;
    public EquipmentType type;
    public int attackBonus;
    public int defenseBonus;
}

[Serializable]
public class StageClearReward
{
    public int stageId;
    public int equipmentId;
}
