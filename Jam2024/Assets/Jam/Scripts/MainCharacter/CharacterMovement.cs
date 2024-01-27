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

    [Header("Sprint")]
    [SerializeField] private float _sprintSpeed = 10f;
    [SerializeField] private float _staminaConsumption = 0.1f;
    [SerializeField] private float _staminaRecharge = 0.1f;
    private float _currentStamina = 1;
    private bool _sprinting;
    private bool _recharginStamina;

    private float _currentAnimationSpeed;
    public Action<float> OnChangeVelocity;
    public Action<float, bool> OnStaminaChange;


    private void Start()
    {
        PlayerInputs.Instance.OnMovementAxis += Move;
        PlayerInputs.Instance.OnSprint += Sprint;
    }

    private void Update()
    {
        if (_sprinting)
        {
            UpdateStamina(-_staminaConsumption * Time.deltaTime);
            if (_currentStamina <= 0)
            {
                _recharginStamina = true;
                _sprinting = false;
            }
        }
        else if (_currentStamina < 1)
        {
            UpdateStamina(_staminaRecharge * Time.deltaTime);

            if (_currentStamina >= 1)
            {
                _recharginStamina = false;
            }
        }
    }

    private void UpdateStamina(float value)
    {
        _currentStamina += value;
        _currentStamina = Mathf.Clamp01(_currentStamina);

        OnStaminaChange?.Invoke(_currentStamina, _recharginStamina);
    }

    private void Sprint(bool sprinting)
    {
        _sprinting = sprinting && !_recharginStamina;
    }

    private void Move(Vector2 moveVector)
    {
        float speed = _sprinting ? _sprintSpeed : _speed;

        _rigidbody2D.velocity = moveVector * speed * Time.deltaTime * 100f;

        if (moveVector != Vector2.zero  && moveVector != _currentDirection)
        {
            OnChangeDirection?.Invoke(moveVector);
            _currentDirection = moveVector;
        }

        float animationSpeed = moveVector.magnitude * (_sprinting ? 2 : 1);

        if (animationSpeed != _currentAnimationSpeed)
        {
            OnChangeVelocity?.Invoke(animationSpeed);
            _currentAnimationSpeed = animationSpeed;
        }

    }
}
