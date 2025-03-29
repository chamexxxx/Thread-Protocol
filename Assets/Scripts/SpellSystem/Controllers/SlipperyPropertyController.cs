using System;
using UnityEngine;
using System;

namespace SpellSystem.Controllers
{
    public class SlipperyPropertyController : MonoBehaviour
    {
        private const string _materialName = "Slippery Physics Material";

        private void Start()
        {
            var physicsMaterial = Resources.Load<PhysicsMaterial>(_materialName);
            
            if (physicsMaterial != null)
            {
                // Применение к текущему коллайдеру
                var collider = GetComponent<Collider>();
                
                if (collider != null)
                {
                    collider.material = physicsMaterial;
                }
                
                Debug.Log("Физический материал успешно загружен и применен.");
            }
            else
            {
                Debug.LogError("Физический материал не найден в папке Resources!");
            }
        }
        
        private void OnDestroy()
        {
            // сбрасываем все установленные значения
        }
    }
}
