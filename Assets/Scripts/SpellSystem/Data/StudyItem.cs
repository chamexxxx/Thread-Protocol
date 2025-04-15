using System.Collections.Generic;
using UnityEngine;

namespace SpellSystem.Data
{
    [System.Serializable]
    public class StudyItem
    {
        public string ItemName;
        public bool IsFeminine;
        
        public List<PropertyType> Properties;
        
        public StudyItem(StudyItem other)
        {
            this.ItemName = other.ItemName;
            this.IsFeminine = other.IsFeminine;
            
            this.Properties = new List<PropertyType>(other.Properties);
        }
    }
}
