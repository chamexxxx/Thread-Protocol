using System.Collections.Generic;
using UnityEngine;

public class ImpulseManager : MonoBehaviour
{
    [SerializeField] private float impulseStrength = 1.1f;
    [SerializeField] private float baseRotationStrength = 1f;
    [SerializeField] private float additionalRotationStrength = 1f;
    [SerializeField] private List<Rigidbody> _impulseTargets = new List<Rigidbody>();

    void Start()
    {
        foreach (Rigidbody rb in _impulseTargets)
        {
            var randomDirection = Random.insideUnitSphere.normalized;
            rb.linearVelocity = randomDirection * impulseStrength;
            
            var randomRotation = Random.insideUnitSphere * additionalRotationStrength;
            rb.angularVelocity = randomRotation + (randomRotation.normalized * baseRotationStrength);
        }
    }
}