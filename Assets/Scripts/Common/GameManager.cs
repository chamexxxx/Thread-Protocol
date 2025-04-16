using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace.Common
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerInput PlayerInput => _playerInput;
        
        [SerializeField] private PlayerInput _playerInput;
        
        private void Awake()
        {
            Instance = this;
        }
    }
}