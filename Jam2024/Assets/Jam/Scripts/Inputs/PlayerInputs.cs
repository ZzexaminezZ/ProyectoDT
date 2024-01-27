using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    public Action<Vector2> OnMovementAxis;
    public Action<bool> OnSprint;
    
    private Vector3 _lastMovement;

    public static PlayerInputs Instance { get; private set; }

    private void Awake()
    {
        Instance = this;   
    }

    private void Update()
    {
        // Movement
        UpdateMovement(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        // Sprint
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            OnSprint?.Invoke(false);
        }
    }

    private void UpdateMovement(Vector2 movement)
    {
        Vector3 normalizedMovement = movement.normalized;

        if (normalizedMovement != Vector3.zero || _lastMovement != Vector3.zero)
        {
            OnMovementAxis(normalizedMovement);
            _lastMovement = normalizedMovement;
        }
    }
}
