// PlayerProgress.cs
using System.Collections.Generic;
using SpellSystem.Data;
using UnityEngine;
namespace SpellSystem
{
    public class PlayerProgress : MonoBehaviour
    {
        public static PlayerProgress Instance { get; private set; }

        public List<StudyItem> studiedItems = new List<StudyItem>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddStudiedItem(StudyItem item)
        {
            if (!studiedItems.Contains(item))
            {
                studiedItems.Add(item);
            }
        }
    }
}