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

    [Header("Dash")]
    [SerializeField] private float _dashCD = 0.7f;
    [SerializeField] private float _dashDistance = 2f;
    [SerializeField] private float _dashTime = 0.5f;
    [SerializeField] private AnimationCurve _dashCurve;
    private float lastDashTime;

    [Header("Stamina")]
    [SerializeField] private float _staminaConsumption = 0.1f;
    [SerializeField] private float _staminaRecharge = 0.1f;

    private float _currentStamina = 1;

    private bool _dashing;
    private bool _recharginStamina;

    private float _currentAnimationSpeed;

    public Action<float> OnChangeVelocity;
    public Action<float, bool> OnStaminaChange;
    public Action<bool> OnDash;


    private void Start()
    {
        PlayerInputs.Instance.OnMovementAxis += Move;
        PlayerInputs.Instance.OnDash += Dash;
    }

    private void Update()
    {
        //dash
        if (_currentStamina < 1)
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

        if (_currentStamina <= 0)
        {
            _recharginStamina = true;
        }

        _currentStamina = Mathf.Clamp01(_currentStamina);

        OnStaminaChange?.Invoke(_currentStamina, _recharginStamina);
    }
    private void Dash()
    {
        if (_dashing || 
            _recharginStamina || 
            DashCD())
            return;


        UpdateStamina(-_staminaConsumption);

        StartCoroutine(DashCoroutine());
    }

    private bool DashCD()
    {
        return Time.time - lastDashTime < _dashCD;
    }

    private IEnumerator DashCoroutine()
    {
        lastDashTime = Time.time;
        _dashing = true;
        OnDash?.Invoke(true);

        Vector3 initPos = transform.position;
        Vector3 endPos = transform.position + (Vector3)_currentDirection * _dashDistance;

        float timer = 0;

        while (timer < _dashTime)
        {
            float t = timer / _dashTime;

            Vector3 target = Vector3.Lerp(initPos, endPos, _dashCurve.Evaluate(t));

            _rigidbody2D.MovePosition(target);

            timer += Time.deltaTime;

            yield return null;
        }

        OnDash?.Invoke(false);
        _dashing = false;
    }


    private void Move(Vector2 moveVector)
    {
        if (_dashing)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }

        _rigidbody2D.velocity = moveVector * _speed;

        if (moveVector != Vector2.zero  && moveVector != _currentDirection)
        {
            OnChangeDirection?.Invoke(moveVector);
            _currentDirection = moveVector;
        }

        if (moveVector.magnitude != _currentAnimationSpeed)
        {
            OnChangeVelocity?.Invoke(moveVector.magnitude);
            _currentAnimationSpeed = moveVector.magnitude;
        }

    }
}
