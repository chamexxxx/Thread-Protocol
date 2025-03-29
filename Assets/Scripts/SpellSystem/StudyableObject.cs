using SpellSystem.Data;
using UnityEngine;

namespace SpellSystem
{
    // StudyableObject.cs
    public class StudyableObject : MonoBehaviour
    {
        public StudyItem itemData;
        public float maxStudyDistance = 5f;
    
        private UIController studySystem;
        
        public bool Studed = false;

        private void Start()
        {
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
        
        /*private bool HasProperty(PropertyType propertyName)
        {
            // Проверяем, что itemData и Properties существуют
            if (itemData == null || itemData.Properties == null)
            {
                return false;
            }

            // Ищем свойство с нужным именем
            foreach (var property in itemData.Properties)
            {
                if (property == propertyName)
                {
                    return true;
                }
            }

            return false;
        }*/
    }
}