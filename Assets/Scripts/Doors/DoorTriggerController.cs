using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DoorTriggerController : MonoBehaviour
    {
        private DoorController _doorController;

        private void Start()
        {
            _doorController = GetComponent<DoorController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
            
            _doorController.OpenDoor();
        }
    }
}