using System.Collections.Generic;
using System.Linq;
using SpellSystem;
using UnityEngine;

namespace Inventory
{
    public class GlobalInventory : MonoBehaviour
    {
        public List<string> items = new();

        public bool HasItem(string code)
        {
            return items.Contains(code);
        }

        public void RemoveItem(string code)
        {
            items.Remove(code);
        }

        public void AddItem(string code)
        {
            items.Add(code);
        }
    }
}