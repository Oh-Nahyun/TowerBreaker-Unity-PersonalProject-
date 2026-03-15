using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OutGamePanel : MonoBehaviour
{
    private void Awake()
    {
        Button play = GetComponentInChildren<Button>();
        play.onClick.AddListener(() => SceneManager.LoadScene(1));
    }
}
