using System.Collections.Generic;
using SpellSystem.Controllers;
using UnityEngine;

namespace SpellSystem.Data
{
    [CreateAssetMenu(fileName = "PropertyDatabase", menuName = "Game/Properties/Database")]
    public class PropertyDatabase : ScriptableObject
    {
        [System.Serializable]
        public class PropertyInfo
        {
            public PropertyType Type;
            public string DisplayName;
            public string DisplayFeminineName;
            public Sprite Icon;
            public List<PropertyType> Antonyms;
            public MonoBehaviour IPropertyController;
        }

        public List<PropertyInfo> AllProperties;

        public PropertyInfo GetPropertyInfo(PropertyType type)
        {
            return AllProperties.Find(p => p.Type == type);
        }
        
        public bool AreAntonyms(PropertyType type1, PropertyType type2)
        {
            var prop1 = GetPropertyInfo(type1);
            var prop2 = GetPropertyInfo(type2);
        
            if (prop1 == null || prop2 == null) 
                return false;

            return prop1.Antonyms.Contains(type2) || prop2.Antonyms.Contains(type1);
        }
    }
}