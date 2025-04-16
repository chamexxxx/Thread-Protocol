using SpellSystem;
using SpellSystem.Data;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerPhysicsController : MonoBehaviour
    {
        [SerializeField] private float _weightLiftingLimit = 3f;
        [SerializeField] private float _pushForce = .2f;

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            var rb = hit.collider.attachedRigidbody;

            if (rb == null || rb.isKinematic || rb.mass > _weightLiftingLimit)
            {
                return;
            }

            var studyableObject = rb.GetComponent<StudyableObject>();

            if (studyableObject == null)
            {
                return;
            }

            if (!(studyableObject.HasProperty(PropertyType.Large) &&
                  studyableObject.HasProperty(PropertyType.Slippery)))
            {
                return;
            }

            // Игнорируем толчки вверх и вниз
            if (hit.moveDirection.y < -0.3f)
            {
                return;
            }

            // Направление толчка
            var pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

            // Применяем силу
            rb.linearVelocity = pushDir * _pushForce;
        }
    }
}
