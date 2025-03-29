using System.Collections.Generic;
using UnityEngine;

public class ObjectImpulse : MonoBehaviour
{
    public float impulseStrength = 5f;
    public float baseRotationStrength = 10f;
    public float additionalRotationStrength = 5f;
    public string targetTag = "Attractable";
    public List<Rigidbody> specificObjects = new List<Rigidbody>();
    public bool applyToTaggedObjects = true;
    public bool applyToSpecificObjects = true;

    void Start()
    {
        List<Rigidbody> objectsToImpulse = new List<Rigidbody>();

        if (applyToSpecificObjects)
        {
            objectsToImpulse.AddRange(specificObjects);
        }
        
        if (applyToTaggedObjects)
        {
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);
            foreach (GameObject obj in taggedObjects)
            {
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null && !objectsToImpulse.Contains(rb))
                {
                    objectsToImpulse.Add(rb);
                }
            }
        }
        
        foreach (Rigidbody rb in objectsToImpulse)
        {
            Vector3 randomDirection = Random.insideUnitSphere.normalized;
            rb.linearVelocity = randomDirection * impulseStrength;
            
            Vector3 randomRotation = Random.insideUnitSphere * additionalRotationStrength;
            rb.angularVelocity = randomRotation + (randomRotation.normalized * baseRotationStrength);
        }
    }
}