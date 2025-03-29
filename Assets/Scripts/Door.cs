using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform leftDoor, rightDoor; // Левая и правая половинки двери
    public Transform openLeft, openRight; // Позиции для открывания
    public Transform closedLeft, closedRight; // Позиции для закрытия
    public float speed = 3f;
    public PressurePlate plate;

    private void OnDestroy()
    {
        plate.Pushed -= OpenDoor;
        plate.UnPushed -= CloseDoor;
    }

    private Vector3 leftTarget, rightTarget;

    private void Start()
    {
        leftTarget = closedLeft.position;
        rightTarget = closedRight.position;
        plate.Pushed += OpenDoor;
        plate.UnPushed += CloseDoor;
    }

    private void Update()
    {
        // Двигаем половинки двери плавно к целевым точкам
        leftDoor.position = Vector3.Lerp(leftDoor.position, leftTarget, Time.deltaTime * speed);
        rightDoor.position = Vector3.Lerp(rightDoor.position, rightTarget, Time.deltaTime * speed);
    }

    public void OpenDoor()
    {
        leftTarget = openLeft.position;
        rightTarget = openRight.position;
    }

    public void CloseDoor()
    {
        leftTarget = closedLeft.position;
        rightTarget = closedRight.position;
    }
}
