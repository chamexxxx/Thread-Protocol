using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpellSystem
{
    public class PhysicsMaterialController : MonoBehaviour
    {
        public static PhysicsMaterialController Instance { get; set; }
        
        public enum Property
        {
            Slippery
        }

        private struct PropertyPhysicsMaterial
        {
            public Property Property;
            public PhysicsMaterial PhysicsMaterial;
        }

        [SerializeField] private List<PropertyPhysicsMaterial> _propertyPhysicsMaterials = new();

        private void Awake()
        {
            Instance = this;
        }
    }
}