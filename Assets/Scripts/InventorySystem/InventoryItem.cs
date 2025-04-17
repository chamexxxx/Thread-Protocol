using System;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    private InventoryManager _inventoryManager;

    private void Start()
    {
        _inventoryManager = FindFirstObjectByType<InventoryManager>();
    }

    public void AddToInventory()
    {
        gameObject.SetActive(false);
        
        _inventoryManager.AddInventoryItem(this);
    }
}
