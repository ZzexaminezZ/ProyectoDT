using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private WinCondition _winCondition;
    [SerializeField] private GameObject _winScreen;
    private void Awake()
    {
        _winCondition.OnWin += () => _winScreen.SetActive(true);
    }
}
