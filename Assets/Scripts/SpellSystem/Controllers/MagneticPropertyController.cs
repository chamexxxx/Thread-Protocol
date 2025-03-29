using System.Collections.Generic;
using SpellSystem.Controllers;
using UnityEngine;

public class MagneticPropertyController : MonoBehaviour, IPropertyController
{
    public float attractionRadius = 5f;
    public float initialSpeed = 1f;
    public float acceleration = 2f;
    public float maxSpeed = 10f;
    public bool isActive = true;
    public string attractableTag = "Attractable";
    
    private List<Rigidbody> attractedObjects = new List<Rigidbody>();

    void Update()
    {
        if (!isActive) return;

        Collider[] colliders = Physics.OverlapSphere(transform.position, attractionRadius);
        foreach (Collider col in colliders)
        {
            if (col.CompareTag(attractableTag))
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null && !attractedObjects.Contains(rb))
                {
                    attractedObjects.Add(rb);
                }
            }
        }

        for (int i = attractedObjects.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = attractedObjects[i];
            if (rb == null || Vector3.Distance(transform.position, rb.position) < 0.1f)
            {
                attractedObjects.RemoveAt(i);
                continue;
            }
            
            Vector3 direction = (transform.position - rb.position).normalized;
            float currentSpeed = Mathf.Min(initialSpeed + acceleration * Time.deltaTime, maxSpeed);
            rb.linearVelocity = direction * currentSpeed;
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, attractionRadius);
    }
}
