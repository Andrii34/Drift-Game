using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarMover : MonoBehaviour,ICarController
{
    private Rigidbody _rb;

    [SerializeField] private Transform _centerOfMass;

    [SerializeField] private Wheel[] _wheels;
    
    [SerializeField] private int _motorForce;
    [SerializeField] private int _brakeForce;

    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private AnimationCurve _steeringCurve;

    private float _speed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
      //  _rb.centerOfMass = _centerOfMass.position;

    }
    public void Update()
    {
        _speed = _rb.velocity.magnitude;
    }
    private void StabilizeCar()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(_rb.velocity);
        if (localVelocity.x > 0.1f || localVelocity.x < -0.1f)
        {
            Vector3 lateralForce = -transform.right * localVelocity.x * 0.7f;
            _rb.AddForce(lateralForce, ForceMode.Acceleration);
        }
    }

    public void Brake(float brakeInput)
    {
        foreach (Wheel wheel in _wheels)
        {
             wheel.WheelCollider.brakeTorque = brakeInput*_brakeForce*(wheel.IsForwardWheel ? 0.7f:0.3f);
            
        }
    }

    public void Steering(float steeringInput)
    {
        float steeringAngle = steeringInput * _steeringCurve.Evaluate(_speed);
        float slipAngle = Vector3.Angle(transform.forward, _rb.velocity - transform.forward);
        if(slipAngle < 120)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, _rb.velocity+transform.forward, Vector3.up);
        }
        steeringAngle = Mathf.Clamp(steeringAngle, -90, 90);
        foreach (Wheel wheel in _wheels)
        {
            if (wheel.IsForwardWheel)
            {
                wheel.WheelCollider.steerAngle = steeringAngle;
            }
        }

    }

    public void Throttle(float gasInput)
    {
        
        
        foreach(Wheel wheel in _wheels)
        {
            wheel.WheelCollider.motorTorque =_motorForce*gasInput;
            wheel.UpdateMeshPosition();
        }
    }

    public void Handbrake(bool handbrakeInput)
    {
        if (handbrakeInput)
        {
            foreach (Wheel wheel in _wheels)
            {
                if (!wheel.IsForwardWheel)
                {
                    wheel.WheelCollider.brakeTorque = _brakeForce * 10000f;
                }
            }

        }
       
    }
}
