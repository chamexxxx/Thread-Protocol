using System;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [Header("Настройки веса")]
    // Минимальный вес, необходимый для активации плиты.
    public float requiredWeight = 10f;

    public GameObject plate;
    [Header("Настройки движения плиты")]
    // Позиция, куда переместится плита при нажатии.
    public Transform pressedPosition;
    // Скорость движения плиты.
    public float moveSpeed = 2f;

    // Исходное положение плиты (устанавливается в Start)
    private Vector3 initialPosition;
    // Флаг, указывающий, нажата ли плита.
    private bool isPressed = false;
    
    public event Action Pushed;
    public event Action UnPushed;

    void Start()
    {
        initialPosition = plate.transform.position;
    }

    // Вызывается, когда другой Collider входит в зону триггера
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Проверяем, что масса объекта превышает порог
            if (rb.mass >= requiredWeight)
            {
                isPressed = true;
                Pushed?.Invoke();
            }
        }
    }

    // Вызывается, когда Collider покидает зону триггера
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Если покидающий объект имеет массу, удовлетворяющую условию
            if (rb.mass >= requiredWeight)
            {
                isPressed = false;
                UnPushed?.Invoke();
            }
        }
    }

    void Update()
    {
        // Плавное перемещение плиты между позициями
        if (isPressed)
        {
            plate.transform.position = Vector3.Lerp(plate.transform.position, pressedPosition.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            plate.transform.position = Vector3.Lerp(plate.transform.position, initialPosition, moveSpeed * Time.deltaTime);
        }
    }
}