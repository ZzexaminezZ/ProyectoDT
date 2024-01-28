using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private UnityAction test;

    private void Start()
    {
        _characterMovement.OnChangeDirection += ChangeDirection;
        _characterMovement.OnChangeVelocity += ChangeVelocity;
    }

    private void ChangeVelocity(float vel)
    {
        _animator.SetFloat("Speed", vel);
    }

    private void ChangeDirection(Vector2 vector)
    {
        _animator.SetFloat("DirX", vector.x);
        _animator.SetFloat("DirY", vector.y);
    }

}
