using System;
using UnityEngine;

namespace SpellSystem.Controllers
{
    public class FrozenPropertyController : MonoBehaviour
    {
        private const string _materialName = "Ice Material";

        private Renderer[] _renderers;
        private Material _defaultMaterial;
        private Collider _collider;
        private bool _isTrigger;

        private void Start()
        {
            _collider = GetComponent<Collider>();

            _isTrigger = _collider.isTrigger;

            _collider.isTrigger = false;
            
            // Получаем все Renderer у объекта и его детей
            _renderers = GetComponentsInChildren<Renderer>();

            if (_renderers.Length == 0)
            {
                Debug.LogWarning($"[FrozenPropertyController] У объекта '{gameObject.name}' и его детей нет Renderer.");
                return;
            }

            // Сохраняем материал первого найденного Renderer
            _defaultMaterial = _renderers[0].material;
            
            var material = Resources.Load<Material>(_materialName);
            if (material == null)
            {
                Debug.LogError($"Материал '{_materialName}' не найден в Resources.");
                return;
            }

            // Применяем новый материал ко всем Renderer
            foreach (Renderer rend in _renderers)
            {
                if (rend != null)
                {
                    rend.material = material;
                }
            }
        }
        
        private void OnDestroy()
        {
            if (_collider != null && _isTrigger)
            {
                _collider.isTrigger = true;
            }

            // Сбрасываем все материалы на стандартные
            if (_renderers == null)
            {
                return;
            }

            foreach (Renderer rend in _renderers)
            {
                if (rend != null)
                {
                    rend.material = _defaultMaterial;
                }
            }
        }
    }
}