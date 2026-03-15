using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    /// <summary>
    /// UI 애니메이터
    /// </summary>
    Animator animator;

    /// <summary>
    /// UI 캔버스 그룹
    /// </summary>
    CanvasGroup canvasGroup;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Button quit = GetComponentInChildren<Button>();
        quit.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    private void Start()
    {
        BindPlayer();
    }

    private void BindPlayer()
    {
        if (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            return;
        }

        player = GameManager.Instance.Player;
        player.onDie += OnPlayerDie;
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.onDie -= OnPlayerDie;
        }
    }

    private void OnPlayerDie(int _)
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        animator.SetTrigger("GameOver");
    }
}
