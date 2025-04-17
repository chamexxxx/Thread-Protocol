using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<InventoryItem> _inventoryItems = new ();

    public void AddInventoryItem(InventoryItem inventoryItem)
    {
        _inventoryItems.Add(inventoryItem);
    }

    public bool RemoveInventoryItem(InventoryItem inventoryItem)
    {
        return _inventoryItems.Remove(inventoryItem);
    }
}
