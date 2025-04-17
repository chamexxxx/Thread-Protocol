using System;
using System.Collections.Generic;
using SpellSystem;
using SpellSystem.Controllers;
using SpellSystem.Data;
using UnityEngine;

[RequireComponent(typeof(StudyableObject))]
public class MagneticPropertyController : MonoBehaviour, IPropertyController
{
    public float _attractionRadius = 10f;
    public float _initialSpeed = 1f;
    public float _acceleration = 2f;
    public float _maxSpeed = 10f;
        
    private readonly List<Rigidbody> _attractedObjects = new ();

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, _attractionRadius);
        
        foreach (Collider col in colliders)
        {
            var rb = col.attachedRigidbody;
            
            if (rb == null || _attractedObjects.Contains(rb))
            {
                continue;
            }

            var studyableObject = col.GetComponent<StudyableObject>();

            if (studyableObject == null || !studyableObject.HasProperty(PropertyType.Steel))
            {
                continue;
            }
            
            _attractedObjects.Add(rb);
        }

        for (int i = _attractedObjects.Count - 1; i >= 0; i--)
        {
            var rb = _attractedObjects[i];
            
            if (rb == null || Vector3.Distance(transform.position, rb.position) < 0.1f)
            {
                _attractedObjects.RemoveAt(i);
                
                continue;
            }
            
            var direction = (transform.position - rb.position).normalized;
            var currentSpeed = Mathf.Min(_initialSpeed + _acceleration * Time.deltaTime, _maxSpeed);
            
            rb.linearVelocity = direction * currentSpeed;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, _attractionRadius);
    }
}
