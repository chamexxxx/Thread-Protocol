using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator), typeof(CharacterController))]
    public class PlayerSpellController : MonoBehaviour
    {
        public event Action SpellSuccess;

        [SerializeField] private int _applySpellDelay = 1000;
        [SerializeField] private GameObject _successParticleSystem;
        [SerializeField] private GameObject _failureParticleSystem;

        private Animator _animator;
        private PlayerMovementController _playerMovementController;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _playerMovementController = GetComponent<PlayerMovementController>();
        }

        public async UniTask Spell(Transform target, bool success)
        {
            await _playerMovementController.RotateToTarget(target);

            _animator.SetTrigger("Spell");

            await UniTask.Delay(_applySpellDelay);

            if (success)
            {
                SpellSuccess?.Invoke();

                _successParticleSystem.SetActive(true);
            }
            else
            {
                _failureParticleSystem.SetActive(true);
            }

            await UniTask.Delay(_applySpellDelay);

            _successParticleSystem.SetActive(false);
            _failureParticleSystem.SetActive(false);
        }
    }
}
