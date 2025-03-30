using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class DoorPlateController : MonoBehaviour
    {
        [SerializeField] private PressurePlate plate;

        private DoorController _doorController;
        
        private void Start()
        {
            _doorController = GetComponent<DoorController>();
            
            plate.Pushed += OnPlatePushed;
            plate.UnPushed += OnPlateUnPushed;
        }

        private void OnDestroy()
        {
            plate.Pushed -= OnPlatePushed;
            plate.UnPushed -= OnPlateUnPushed;
        }

        private void OnPlatePushed()
        {
            _doorController.OpenDoor();
        }

        private void OnPlateUnPushed()
        {
            _doorController.CloseDoor();
        }
    }
}
