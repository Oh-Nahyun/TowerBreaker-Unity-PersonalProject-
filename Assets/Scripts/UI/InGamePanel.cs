using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    /// <summary>
    /// 게임 스테이지 텍스트
    /// </summary>
    TMP_Text gameStageText;

    /// <summary>
    /// 플레이어 체력 텍스트
    /// </summary>
    TMP_Text playerHealthText;

    private void Awake()
    {
        gameStageText = transform.Find("GameStageText").GetComponent<TMP_Text>();
        playerHealthText = transform.Find("PlayerHealthText").GetComponent<TMP_Text>();

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

    public void UpdateGameStageUI(int gameStage)
    {
        gameStageText.text = $"[ Stage {gameStage} ]";
    }

    public void UpdatePlayerHealthUI(int currentHealth)
    {
        playerHealthText.text = $"Player HP : {currentHealth} / 100";
    }
}
