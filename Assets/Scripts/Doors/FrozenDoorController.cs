using System;
using SpellSystem;
using SpellSystem.Data;
using UnityEngine;

public class FrozenDoorController : MonoBehaviour
{
    private DoorController _doorController;
    private StudyableObject _studyableObject;
    
    void Start()
    {
        _doorController = GetComponent<DoorController>();
        _studyableObject = GetComponent<StudyableObject>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_doorController.IsOpened)
        {
            return;
        }
        
        if (!other.CompareTag("Player"))
        {
            return;
        }
        
        if (_studyableObject.HasProperty(PropertyType.Frozen))
        {
            return;
        }
        
        _doorController.OpenDoor();
    }
}
