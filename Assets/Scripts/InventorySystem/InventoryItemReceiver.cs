using System;
using UnityEngine;

public class InventoryItemReceiver : MonoBehaviour
{
    public bool IsActivated => _isActivated;

    public event Action Activated;
    
    [SerializeField] private InventoryItem _inventoryItem;

    private InventoryManager _inventoryManager;
    private bool _isActivated = false;
    
    private void Start()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public bool TryActivate()
    {
        if (_isActivated)
        {
            return true;
        }
        
        var success = _inventoryManager.RemoveInventoryItem(_inventoryItem);

        if (success)
        {
            _isActivated = true;
            Activated?.Invoke();
        }

        return success;
    }
}
