using UnityEngine;

namespace SpellSystem
{
    public class FreeCameraMouseLook : MonoBehaviour
    {
        [Header("Настройки вращения")]
        [Tooltip("Чувствительность мыши")]
        public float mouseSensitivity = 100f;
        
        [Header("Ограничения угла")]
        [Tooltip("Минимальный угол наклона вниз")]
        public float minVerticalAngle = -90f;
        [Tooltip("Максимальный угол наклона вверх")]
        public float maxVerticalAngle = 90f;
        
        private float xRotation = 0f;
        private float yRotation = 0f;

        void Start()
        {
            // Фиксируем курсор в центре экрана
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Инициализируем текущие углы поворота
            Vector3 currentRotation = transform.localEulerAngles;
            xRotation = currentRotation.x;
            yRotation = currentRotation.y;
        }

        void Update()
        {
            // Получаем ввод мыши
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // Изменяем углы поворота
            yRotation += mouseX;  // Горизонтальное вращение
            xRotation -= mouseY;  // Вертикальное вращение
            
            // Ограничиваем вертикальный угол
            xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
            
            // Применяем вращение
            transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        }

        // Для временного отключения управления
        public void SetLookEnabled(bool enabled)
        {
            enabled = enabled;
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enabled;
        }
    }
}