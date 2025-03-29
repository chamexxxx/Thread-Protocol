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
                Debug.LogError("StudySystem not found in scene!");
            }
        }
        
        public bool HasProperty(PropertyType propertyType)
        {
            if (itemData == null || itemData.Properties == null)
                return false;

            return itemData.Properties.Contains(propertyType);
        }
        
    }
}