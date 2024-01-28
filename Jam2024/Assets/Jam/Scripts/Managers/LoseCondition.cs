using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    [SerializeField] private PlayerLife _playerLife;

    public Action OnLose;

    private void Awake()
    {
        _playerLife.OnPlayerCatched += Lose;
    }

    private void Lose()
    {
        OnLose?.Invoke();
    }
}
