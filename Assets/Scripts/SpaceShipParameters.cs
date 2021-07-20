using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpaceShipParameters : ScriptableObject
{
    public float ThrustForce;
    public float TorqueForce;
    public float MaxLinearVelocity;
    public float MaxAngularVelocity; 
}
