using System;
using System.Collections.Generic;
using SpellSystem.Controllers;
using SpellSystem.Data;
using UnityEngine;

namespace SpellSystem
{
    public class PropertyControllerAttacher : MonoBehaviour
    {
        private Dictionary<PropertyType, Type> _controllers = new()
        {
            { PropertyType.Large, typeof(LargePropertyController) },
            { PropertyType.Slippery, typeof(SlipperyPropertyController) },
            { PropertyType.Stone, typeof(StonePropertyController) },
            { PropertyType.Glass, typeof(GlassPropertyController) },
            { PropertyType.Frozen, typeof(FrozenPropertyController) },
            { PropertyType.Hot, typeof(HotPropertyController) },
            { PropertyType.Gold, typeof(GoldPropertyController) },
            { PropertyType.Magnetic, typeof(MagneticPropertyController) },
            { PropertyType.Steel, typeof(SteelPropertyController) }
        };
        
        public void AddPropertyController(PropertyType propertyType)
        {
            if (_controllers.TryGetValue(propertyType, out Type controllerType))
            {
                if (gameObject.GetComponent(controllerType) == null)
                {
                    gameObject.AddComponent(controllerType);
                }
            }
            else
            {
                Debug.LogWarning($"Нет контроллера для {propertyType}");
            }
        }
        
        public void RemovePropertyController(PropertyType propertyType)
        {
            if (_controllers.TryGetValue(propertyType, out Type controllerType))
            {
                var component = gameObject.GetComponent(controllerType);
                
                if (component != null)
                {
                    Destroy(component);
                }
            }
            else
            {
                Debug.LogWarning($"Нет контроллера для {propertyType}");
            }
        }

    }
}