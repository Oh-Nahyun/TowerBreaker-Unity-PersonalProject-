using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    private void Awake()
    {
        Button hardSkill = transform.Find("HardSkillButton").GetComponent<Button>();
        Button rangeSkill = transform.Find("RangeSkillButton").GetComponent<Button>();
        Button stabSkill = transform.Find("StabSkillButton").GetComponent<Button>();
        Button attack = transform.Find("AttackButton").GetComponent<Button>();
        Button shield = transform.Find("ShieldButton").GetComponent<Button>();
        Button move = transform.Find("MoveButton").GetComponent<Button>();

        hardSkill.onClick.AddListener(() => GameManager.Instance.Player.SetHardSkillInput(true));
        rangeSkill.onClick.AddListener(() => GameManager.Instance.Player.SetRangeSkillInput(true));
        stabSkill.onClick.AddListener(() => GameManager.Instance.Player.SetStabSkillInput(true));

        AddPointerEvent(attack, EventTriggerType.PointerDown, () => GameManager.Instance.Player.SetNormalAttackInput(true));
        AddPointerEvent(attack, EventTriggerType.PointerUp, () => GameManager.Instance.Player.SetNormalAttackInput(false));

        shield.onClick.AddListener(() => GameManager.Instance.Player.SetDefenseInput(true));
        move.onClick.AddListener(() => GameManager.Instance.Player.SetMoveInput(true));
    }

    private void AddPointerEvent(Button button, EventTriggerType eventTriggerType, Action action)
    {
        EventTrigger trigger = button.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = button.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventTriggerType;
        entry.callback.AddListener((_) => action());

        trigger.triggers.Add(entry);
    }
}
