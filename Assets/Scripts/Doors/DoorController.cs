using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Transform leftDoor, rightDoor; // Левая и правая половинки двери
    [SerializeField] private Transform openLeft, openRight; // Позиции для открывания
    [SerializeField] private Transform closedLeft, closedRight; // Позиции для закрытия
    [SerializeField] private float speed = 3f;

    private Vector3 leftTarget, rightTarget;
    
    private bool _isOpened = false;
    
    public bool IsOpened => _isOpened;

    private void Start()
    {
        leftTarget = closedLeft.position;
        rightTarget = closedRight.position;
    }

    private void Update()
    {
        // Двигаем половинки двери плавно к целевым точкам
        leftDoor.position = Vector3.Lerp(leftDoor.position, leftTarget, Time.deltaTime * speed);
        rightDoor.position = Vector3.Lerp(rightDoor.position, rightTarget, Time.deltaTime * speed);
    }

    public void OpenDoor()
    {
        _isOpened = true;
        
        leftTarget = openLeft.position;
        rightTarget = openRight.position;
    }

    public void CloseDoor()
    {
        _isOpened = false;
        
        leftTarget = closedLeft.position;
        rightTarget = closedRight.position;
    }
}
