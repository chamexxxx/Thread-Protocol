using Cysharp.Threading.Tasks;
using SpellSystem;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public Transform teleportTarget; // Целевая точка телепортации
    public GameObject player; // Ссылка на объект персонажа
    public Material waterMaterial; // Исходный шейдер воды
    public Material replacementMaterial; // Материал, заменяющий шейдер
    public PhysicsMaterial frozenPhysicsMaterial; // Физический материал для замороженной воды
    public bool isFrozen = false; // Переключатель состояния воды в редакторе
    private Material originalMaterial; // Сохранение оригинального материала
    private Collider waterCollider;
    private Renderer waterRenderer;
    private StudyableObject _studyableObject;

    private void Start()
    {
        _studyableObject = GetComponent<StudyableObject>();
        
        waterCollider = GetComponent<Collider>();
        waterRenderer = GetComponent<Renderer>();
        if (waterRenderer != null)
        {
            originalMaterial = waterRenderer.material;
        }
        
        // UpdateWaterState();
    }

    private void OnValidate()
    {
        UpdateWaterState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }

    private async UniTask TeleportPlayer()
    {
        Debug.Log("TeleportPlayer 1");
        
        if (teleportTarget == null || player == null)
        {
            return;
        }
        
        Debug.Log("TeleportPlayer 2");
        
        var animator = player.GetComponent<Animator>();
            
        if (animator != null)
        {
            animator.enabled = false; // Останавливаем анимации
        }

        var rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.position = teleportTarget.position;
        }

        var characterController = player.GetComponent<CharacterController>();
        
        await UniTask.Delay(500);

        characterController.enabled = false;
        
        player.transform.position = teleportTarget.position;

        characterController.enabled = true;

        if (animator != null)
        {
            animator.enabled = true; // Включаем анимации обратно
        }
    }

    public void DisableWaterEffect()
    {
        if (waterCollider != null)
        {
            waterCollider.isTrigger = false;
            if (frozenPhysicsMaterial != null)
            {
                waterCollider.material = frozenPhysicsMaterial;
            }
        }
        if (waterRenderer != null && replacementMaterial != null)
        {
            waterRenderer.material = replacementMaterial;
        }
    }

    public void EnableWaterEffect()
    {
        if (waterCollider != null)
        {
            waterCollider.isTrigger = true;
            waterCollider.material = null; // Убираем физический материал
        }
        if (waterRenderer != null && originalMaterial != null)
        {
            waterRenderer.material = originalMaterial;
        }
    }

    public void UpdateWaterState()
    {
        if (isFrozen)
        {
            DisableWaterEffect();
        }
        else
        {
            EnableWaterEffect();
        }
    }
}
