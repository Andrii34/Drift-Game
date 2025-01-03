using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CarInputController : MonoBehaviour
{
   private ICarController _carController;
    private CarInput _carInput;
    private bool _isHandbrakeHeld;
    private Rigidbody _playerRB;
    private float _brakeInput;
    private float _steeringInput;
    
    private float _gasInput;
    private void Awake()
    {
        _carInput = new CarInput();
       
        _carController = GetComponent<ICarController>();
        _playerRB = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        if (_carInput == null) 
        {
            _carInput = new CarInput();
        }

        _carInput.Enable();
        _carInput.Car.handbrake.performed += ctx => _isHandbrakeHeld = true;
        _carInput.Car.handbrake.canceled += ctx => _isHandbrakeHeld = false;
    }
    private void Update()
    {
        SteeringInput();
        ThrottleInput();
        BrakeInputModifier();

    }
    private void FixedUpdate()
    {
        _carController.Handbrake(_isHandbrakeHeld);
        
        _carController.Brake(_brakeInput);
        _carController.Steering(_steeringInput);
        _carController.Throttle(_gasInput);
    }
    private void BrakeInputModifier()
    {
        float movingDirection = Vector3.Dot(transform.forward, _playerRB.velocity);
        if (movingDirection < -0.5f && _gasInput > 0)
        {
            _brakeInput = Mathf.Abs(_gasInput);
        }
        else if (movingDirection > 0.5f && _gasInput < 0)
        {
            _brakeInput = Mathf.Abs(_gasInput);
        }
        else
        {
            _brakeInput = 0;
        }
    }


    private void OnDisable()
    {
        _carInput.Car.handbrake.performed -= ctx => _isHandbrakeHeld = true;
        _carInput.Car.handbrake.canceled -= ctx => _isHandbrakeHeld = false;
    }
    private void SteeringInput()
    {
        _steeringInput = _carInput.Car.Steering.ReadValue<float>();

    }
    private void ThrottleInput()
    {
        _gasInput = _carInput.Car.Throttle.ReadValue<float>();
    }

}
