using UnityEngine;

namespace SpellSystem.Data
{
    [System.Serializable]
    public class StudyItem
    {
        public string ItemName;
        public bool IsFeminine;
        
        public PropertyType[] Properties;
    }
}
