using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private Rigidbody2D _rigidbody2D;

    private Vector2 _currentDirection;
    public Action<Vector2> OnChangeDirection;

    private float _currentVelocityNormalized;
    public Action<float> OnChangeVelocity;


    private void Start()
    {
        PlayerInputs.Instance.OnMovementAxis += Move;
    }

    private void Move(Vector2 moveVector)
    {
        _rigidbody2D.velocity = moveVector * _speed;

        if (moveVector != Vector2.zero  && moveVector != _currentDirection)
        {
            OnChangeDirection?.Invoke(moveVector);
            _currentDirection = moveVector;
        }

        if (moveVector.magnitude != _currentVelocityNormalized)
        {
            OnChangeVelocity?.Invoke(moveVector.magnitude);
            _currentVelocityNormalized = moveVector.magnitude;
        }

    }
}
