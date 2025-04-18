using System;
using UnityEngine;

[RequireComponent(typeof(DoorController), typeof(InventoryItemReceiver))]
public class DoorKeyReceiver : MonoBehaviour
{
    private DoorController _doorController;
    private InventoryItemReceiver _inventoryItemReceiver;

    private void Start()
    {
        _doorController = GetComponent<DoorController>();
        _inventoryItemReceiver = GetComponent<InventoryItemReceiver>();

        _inventoryItemReceiver.Activated += OnInventoryItemReceiverActivated;
    }

    private void OnInventoryItemReceiverActivated()
    {
        _doorController.OpenDoor();
    }
}
