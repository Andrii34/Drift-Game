using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarController 
{
    public void Steering(float steeringInput);
    public void Throttle(float gasInput);
     public void Brake(float brakeInput);
    public void Handbrake(bool handbrakeInput);



}
