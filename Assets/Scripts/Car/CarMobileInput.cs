using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarMobileInput : MonoBehaviour
{
    [SerializeField] private PressableButton _brakeButton;
    [SerializeField] private PressableButton _gasButton;
    [SerializeField] private PressableButton _leftButton;
    [SerializeField] private PressableButton _rightButton;
    [SerializeField] private PressableButton _handbrakeButton;

    private ICarController _carController;
    private Rigidbody _playerRB;
    private float _gasInput;
    private float _brakeInput;
    private float _steeringInput;
    private bool _isHandbrakeHeld;
    void Start()
    {
        _carController = GetComponent<ICarController>();
        _playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        SteeringInput();
        ThrottleInput();
        BrakeInputModifier();
        _isHandbrakeHeld = _brakeButton.IsPressed;
        _carController.Handbrake(_isHandbrakeHeld);

        _carController.Brake(_brakeInput);
        _carController.Steering(_steeringInput);
        _carController.Throttle(_gasInput);
        
    }

    private void FixedUpdate()
    {
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
    private void SteeringInput()
    {
        _steeringInput = 0;
        if (_rightButton.IsPressed)
        {
            _steeringInput += _rightButton.DampenPress;
        }
        if (_leftButton.IsPressed)
        {
            _steeringInput -= _leftButton.DampenPress;
        }

    }
    private void ThrottleInput()
    {
        _gasInput = 0;
        if (_gasButton.IsPressed)
        {
            _gasInput += _gasButton.DampenPress;
        }
       if (_brakeButton.IsPressed)
        {
            _gasInput -= _brakeButton.DampenPress;
        }
    }
}
