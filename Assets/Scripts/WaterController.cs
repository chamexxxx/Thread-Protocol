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

    void Start()
    {
        waterCollider = GetComponent<Collider>();
        waterRenderer = GetComponent<Renderer>();
        if (waterRenderer != null)
        {
            originalMaterial = waterRenderer.material;
        }
        UpdateWaterState();
    }

    void OnValidate()
    {
        UpdateWaterState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer()
    {
        if (teleportTarget != null && player != null)
        {
            Animator animator = player.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = false; // Останавливаем анимации
            }

            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.position = teleportTarget.position;
            player.transform.position = teleportTarget.position;

            if (animator != null)
            {
                animator.enabled = true; // Включаем анимации обратно
            }
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
