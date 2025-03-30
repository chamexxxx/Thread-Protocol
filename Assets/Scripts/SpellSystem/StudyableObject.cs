using Inventory;
using JetBrains.Annotations;
using SpellSystem.Controllers;
using SpellSystem.Data;
using UnityEngine;

namespace SpellSystem
{
    public class StudyableObject : MonoBehaviour
    {
        public bool pickable; // можно ли положить в инвентарь
        [CanBeNull] public string inventoryCode;
        public StudyItem itemData;
        public float maxStudyDistance = 5f;
    
        private UIController studySystem;
        private GlobalInventory inventory;
        
        public bool Studed = false;

        private void Start()
        {
            inventory = FindObjectOfType<GlobalInventory>();
            studySystem = FindObjectOfType<UIController>();
            if (studySystem == null)
            {
                Debug.LogError("UIController not found in scene!");
            }
        }
        
        public bool HasProperty(PropertyType propertyType)
        {
            if (itemData == null || itemData.Properties == null)
                return false;

            return itemData.Properties.Contains(propertyType);
        }

        public void CheckPropertyController(PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.Slippery:
                    if (!gameObject.GetComponent<SlipperyPropertyController>())
                        gameObject.AddComponent<SlipperyPropertyController>();
                    break;
    
                case PropertyType.Stone:
                    if (!gameObject.GetComponent<StonePropertyController>())
                        gameObject.AddComponent<StonePropertyController>();
                    break;
    
                case PropertyType.Glass:
                    if (!gameObject.GetComponent<GlassPropertyController>())
                        gameObject.AddComponent<GlassPropertyController>();
                    break;
    
                case PropertyType.Large:
                    if (!gameObject.GetComponent<LargePropertyController>())
                        gameObject.AddComponent<LargePropertyController>();
                    break;
    
                case PropertyType.Frozen:
                    if (!gameObject.GetComponent<FrozenPropertyController>())
                        gameObject.AddComponent<FrozenPropertyController>();
                    break;
    
                case PropertyType.Hot:
                    if (!gameObject.GetComponent<HotPropertyController>())
                        gameObject.AddComponent<HotPropertyController>();
                    break;
    
                case PropertyType.Gold:
                    if (!gameObject.GetComponent<GoldPropertyController>())
                        gameObject.AddComponent<GoldPropertyController>();
                    break;
    
                case PropertyType.Magnetic:
                    if (!gameObject.GetComponent<MagneticPropertyController>())
                        gameObject.AddComponent<MagneticPropertyController>();
                    break;
    
                case PropertyType.Steel:
                    if (!gameObject.GetComponent<SteelPropertyController>())
                        gameObject.AddComponent<SteelPropertyController>();
                    break;
    
                default:
                    Debug.LogWarning($"Неизвестный тип свойства: {propertyType}");
                    break;
            }
        }

        public void AddProperty(PropertyType propertyType)
        {
            itemData.Properties.Add(propertyType);
            
            switch (propertyType)
            {
                case PropertyType.Slippery:
                    gameObject.AddComponent<SlipperyPropertyController>();
                    break;
            
                case PropertyType.Stone:
                    gameObject.AddComponent<StonePropertyController>();
                    break;
            
                case PropertyType.Glass:
                    gameObject.AddComponent<GlassPropertyController>();
                    break;
            
                case PropertyType.Large:
                    gameObject.AddComponent<LargePropertyController>();
                    break;
            
                case PropertyType.Frozen:
                    gameObject.AddComponent<FrozenPropertyController>();
                    break;
            
                case PropertyType.Hot:
                    gameObject.AddComponent<HotPropertyController>();
                    break;
            
                case PropertyType.Gold:
                    gameObject.AddComponent<GoldPropertyController>();
                    break;
            
                case PropertyType.Magnetic:
                    gameObject.AddComponent<MagneticPropertyController>();
                    break;
            
                case PropertyType.Steel:
                    gameObject.AddComponent<SteelPropertyController>();
                    break;
            
                default:
                    Debug.LogWarning($"Неизвестный тип свойства: {propertyType}");
                    break;
            }
            
            //Вызвать методы у контроллера
    
        }
        
        public void RemoveProperty(PropertyType propertyType)
        {
            itemData.Properties.Add(propertyType);

            switch (propertyType)
            {
                case PropertyType.Slippery:
                    RemoveComponent<SlipperyPropertyController>();
                    break;
        
                case PropertyType.Stone:
                    RemoveComponent<StonePropertyController>();
                    break;
        
                case PropertyType.Glass:
                    RemoveComponent<GlassPropertyController>();
                    break;
        
                case PropertyType.Large:
                    RemoveComponent<LargePropertyController>();
                    break;
        
                case PropertyType.Frozen:
                    RemoveComponent<FrozenPropertyController>();
                    break;
        
                case PropertyType.Hot:
                    RemoveComponent<HotPropertyController>();
                    break;
        
                case PropertyType.Gold:
                    RemoveComponent<GoldPropertyController>();
                    break;
        
                case PropertyType.Magnetic:
                    RemoveComponent<MagneticPropertyController>();
                    break;
        
                case PropertyType.Steel:
                    RemoveComponent<SteelPropertyController>();
                    break;
        
                default:
                    Debug.LogWarning($"Неизвестный тип свойства: {propertyType}");
                    break;
            }

            void RemoveComponent<T>() where T : MonoBehaviour
            {
                var component = gameObject.GetComponent<T>();
                if (component != null)
                {
                    Destroy(component);
                }
            }

        }

        public void Pickup()
        {
            inventory.AddItem(inventoryCode);
            Destroy(gameObject);
        }
    }
}