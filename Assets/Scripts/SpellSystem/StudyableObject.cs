using System;
using SpellSystem.Controllers;
using SpellSystem.Data;
using UnityEngine;

namespace SpellSystem
{
    public class StudyableObject : MonoBehaviour
    {
        [SerializeField] private bool _constrained = false;
        
        public StudyItem itemData;
        private StudyItem _startItemData;
        public float maxStudyDistance = 5f;

        private Vector3 _initialScale;

        public Vector3 InitialScale => _initialScale;
    
        private UIController studySystem;
        
        public bool Studed = false;
        public bool Constrained => _constrained;
        public StudyItem StartItemData => _startItemData;

        private PropertyControllerAttacher _propertyControllerAttacher;

        private void Awake()
        {
            _propertyControllerAttacher = GetComponent<PropertyControllerAttacher>();
            
            Debug.Log(gameObject.name + " - " + _propertyControllerAttacher);
        }

        private void Start()
        {
            studySystem = FindObjectOfType<UIController>();
            if (studySystem == null)
            {
                Debug.LogError("UIController not found in scene!");
                return;
            }

            _startItemData = new StudyItem(itemData);
            _initialScale = transform.localScale;
        }
        
        public bool HasProperty(PropertyType propertyType)
        {
            if (itemData == null || itemData.Properties == null)
                return false;

            return itemData.Properties.Contains(propertyType);
        }

        public void CheckPropertyController(PropertyType propertyType)
        {
            _propertyControllerAttacher.AddPropertyController(propertyType);
        }

        public void AddProperty(PropertyType propertyType)
        {
            itemData.Properties.Add(propertyType);
            
            _propertyControllerAttacher.AddPropertyController(propertyType);
            
            //Вызвать методы у контроллера
        }
        
        public void RemoveProperty(PropertyType propertyType)
        {
            itemData.Properties.Remove(propertyType);

            _propertyControllerAttacher.RemovePropertyController(propertyType);
        }
        
    }
}