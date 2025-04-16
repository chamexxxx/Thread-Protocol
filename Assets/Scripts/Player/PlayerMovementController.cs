using Cysharp.Threading.Tasks;
using DefaultNamespace.Common;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController), typeof(Animator))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] private float _gravity = -9.81f;

        [Header("Camera settings")] [SerializeField]
        private Transform _playerCamera;

        [Header("Movement settings")] [SerializeField]
        private float _moveSpeed = 1.5f;

        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private float _rotationThreshold = 1f;

        private CharacterController _characterController;
        private Animator _animator;

        private InputAction _movementAction;

        private Vector3 _velocity;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();

            var playerInput = GameManager.Instance.PlayerInput;

            _movementAction = playerInput.actions.FindAction("Move");
        }

        private void Update()
        {
            // Проверяем, проигрывается ли анимация "Spell"
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Spell"))
            {
                return;
            }
            
            var movementValue = _movementAction.ReadValue<Vector2>();

            // Направление по камере (без учёта высоты)
            var cameraForward = _playerCamera.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            var cameraRight = _playerCamera.right;
            cameraRight.y = 0;
            cameraRight.Normalize();

            // Направление движения относительно камеры
            var move = cameraForward * movementValue.y + cameraRight * movementValue.x;

            // Применение движения по горизонтали
            var horizontalMove = _moveSpeed * Time.deltaTime * move;

            // Применение гравитации
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f; // Маленькое отрицательное значение, чтобы прилипал к земле
            }

            _velocity.y += _gravity * Time.deltaTime;

            // Итоговое движение
            var totalMove = horizontalMove + _velocity * Time.deltaTime;
            _characterController.Move(totalMove);

            // Анимация и поворот
            _animator.SetFloat("Speed", move.magnitude);

            if (move.magnitude > 0.1f)
            {
                var targetRotation = Quaternion.LookRotation(move);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        public async UniTask RotateToTarget(Transform target)
        {
            if (target == null)
            {
                return;
            }

            while (true)
            {
                // Направление к цели
                var direction = (target.position - transform.position).normalized;

                direction.y = 0f; // Игнорируем вертикаль

                // Поворот в нужную сторону
                var targetRotation = Quaternion.LookRotation(direction);

                // Плавный поворот
                transform.rotation = Quaternion.Slerp(
                    transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime
                );

                // Угол между текущим и целевым поворотом
                var angle = Quaternion.Angle(transform.rotation, targetRotation);

                // Если угол достаточно мал — завершить
                if (angle < _rotationThreshold)
                {
                    break;
                }

                await UniTask.Yield();
            }
        }
    }
}
