using System.Collections;
using JetBrains.Annotations;
using UnityEngine;

namespace Invector.vCharacterController
{
    public class vThirdPersonInput : MonoBehaviour
    {
        #region Variables       

        [Header("Controller Input")]
        public string horizontalInput = "Horizontal";
        public string verticallInput = "Vertical";
        public KeyCode jumpInput = KeyCode.Space;
        public KeyCode strafeInput = KeyCode.Tab;
        public KeyCode sprintInput = KeyCode.LeftShift;
        public KeyCode spellInput = KeyCode.R;

        [Header("Camera Input")]
        public string rotateCameraXInput = "Mouse X";
        public string rotateCameraYInput = "Mouse Y";

        [HideInInspector] public vThirdPersonController cc;
        [HideInInspector] public vThirdPersonCamera tpCamera;
        [HideInInspector] public Camera cameraMain;
        
        [CanBeNull] public Transform target;          // Цель, на которую будем поворачиваться
        public float rotationSpeed = 5f;  // Скорость поворота
        private bool isRotating = false;
        
        private Coroutine rotateCoroutine;

        public bool block = false;
        #endregion

        protected virtual void Start()
        {
            InitilizeController();
            InitializeTpCamera();
        }

        protected virtual void FixedUpdate()
        {
            cc.UpdateMotor();               // updates the ThirdPersonMotor methods
            cc.ControlLocomotionType();     // handle the controller locomotion type and movespeed
            cc.ControlRotationType();       // handle the controller rotation type
        }

        protected virtual void Update()
        {
            if (target != null && !isRotating)
            {
                if (rotateCoroutine != null)
                    StopCoroutine(rotateCoroutine); // Останавливаем предыдущую корутину (если есть)

                rotateCoroutine = StartCoroutine(RotateTowardsTarget());
            }
            
            if (!block)
            {
                InputHandle();                  // update the input methods
                cc.UpdateAnimator();            // updates the Animator Parameters
            }
        }

        public virtual void SetRotateTarget(GameObject newTarget)
        {
            target = newTarget.transform;
        }

        IEnumerator RotateTowardsTarget()
        {
            isRotating = true;
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Убираем наклон вверх/вниз

            if (direction.magnitude > 0.01f)
            {
                Quaternion startRotation = transform.rotation;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                float t = 0;

                while (t < 1)
                {
                    t += Time.deltaTime * rotationSpeed;
                    transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
                    yield return null;
                }
            }

            target = null;
            isRotating = false;
        }

        public virtual void OnAnimatorMove()
        {
            cc.ControlAnimatorRootMotion(); // handle root motion animations 
        }

        #region Basic Locomotion Inputs

        protected virtual void InitilizeController()
        {
            cc = GetComponent<vThirdPersonController>();

            if (cc != null)
                cc.Init();
        }

        protected virtual void InitializeTpCamera()
        {
            if (tpCamera == null)
            {
                tpCamera = FindObjectOfType<vThirdPersonCamera>();
                if (tpCamera == null)
                    return;
                if (tpCamera)
                {
                    tpCamera.SetMainTarget(this.transform);
                    tpCamera.Init();
                }
            }
        }

        protected virtual void InputHandle()
        {
            MoveInput();
            CameraInput();
            SprintInput();
            StrafeInput();
            JumpInput();
            SpellInput();
        }

        public virtual void MoveInput()
        {
            cc.input.x = Input.GetAxis(horizontalInput);
            cc.input.z = Input.GetAxis(verticallInput);
        }

        protected virtual void CameraInput()
        {
            if (!cameraMain)
            {
                if (!Camera.main) Debug.Log("Missing a Camera with the tag MainCamera, please add one.");
                else
                {
                    cameraMain = Camera.main;
                    cc.rotateTarget = cameraMain.transform;
                }
            }

            if (cameraMain)
            {
                cc.UpdateMoveDirection(cameraMain.transform);
            }

            if (tpCamera == null)
                return;

            var Y = Input.GetAxis(rotateCameraYInput);
            var X = Input.GetAxis(rotateCameraXInput);

            tpCamera.RotateCamera(X, Y);
        }

        protected virtual void StrafeInput()
        {
            if (Input.GetKeyDown(strafeInput))
                cc.Strafe();
        }

        protected virtual void SprintInput()
        {
            if (Input.GetKeyDown(sprintInput))
                cc.Sprint(true);
            else if (Input.GetKeyUp(sprintInput))
                cc.Sprint(false);
        }

        protected virtual void SpellInput()
        {
            if (Input.GetKeyDown(spellInput))
                cc.Spell();
        }

        /// <summary>
        /// Conditions to trigger the Jump animation & behavior
        /// </summary>
        /// <returns></returns>
        protected virtual bool JumpConditions()
        {
            return cc.isGrounded && cc.GroundAngle() < cc.slopeLimit && !cc.isJumping && !cc.stopMove;
        }

        /// <summary>
        /// Input to trigger the Jump 
        /// </summary>
        protected virtual void JumpInput()
        {
            if (Input.GetKeyDown(jumpInput) && JumpConditions())
                cc.Jump();
        }

        #endregion       
    }
}