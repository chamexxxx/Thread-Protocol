using System;
using UnityEngine;

public class CoffeeMachine : MonoBehaviour
{
    [SerializeField] private Transform _shutter;
    [SerializeField] private Vector3 _targetShutterPosition;
    [SerializeField] private float _loweringSpeed = 1.0f;
    
    private InventoryItemReceiver _inventoryItemReceiver;
    
    private bool _isLowering = false;
    
    private void Start()
    {
        _inventoryItemReceiver = GetComponent<InventoryItemReceiver>();

        _inventoryItemReceiver.Activated += OnInventoryItemReceiverActivated;
    }
    
    private void Update()
    {
        if (!_isLowering)
        {
            return;
        }
        
        _shutter.localPosition = Vector3.MoveTowards(
            _shutter.localPosition, _targetShutterPosition, _loweringSpeed * Time.deltaTime
        );

        if (_shutter.localPosition == _targetShutterPosition)
        {
            _isLowering = false;
        }
    }

    // Метод для запуска опускания
    public void LowerShutter()
    {
        _isLowering = true;
    }

    private void OnInventoryItemReceiverActivated()
    {
        LowerShutter();
    }
}
