using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseScreen : MonoBehaviour
{
    [SerializeField] private LoseCondition _loseCondition;

    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        _loseCondition.OnLose += ShowLoseScreen;
        _restartButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowLoseScreen()
    {
        _loseScreen.SetActive(true);
    }
}
