using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] private Invulnerability _invulnerability;
    public Action OnPlayerCatched;

    public bool CatchPlayer()
    {
        if (!_invulnerability.IsInvulnerable)
        {
            OnPlayerCatched?.Invoke();
        }
        return _invulnerability.IsInvulnerable;
    }
}
