using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        #region Types
        private enum View
        {
            ThirdPersonFront,
            ThirdPersonBack,
            FirstPerson
        };
        [Serializable]
        private struct BoostCurve
        {
            public AnimationCurve curve;
            public float duration;
            public bool Done { get; private set; }
            private float _boostTime;

            public void SetBoostTime(float now) 
            {
                _boostTime = now;
            }
            public float GetBoostTime(float now)
            {
                Done = (now - _boostTime) > duration;
                if (Done) return 1;
                return curve.Evaluate((now - _boostTime) / duration);
            }
        }
        #endregion
        
        #region Properties
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject head;
        [SerializeField] private InputActionReference movingAction;
        [SerializeField] private InputActionReference lookingAction;
        [SerializeField] private InputActionReference switchCamera;
        [SerializeField] private BoostCurve speedup;

        private const float MinYRotation = -90f;
        private const float MaxYRotation = 55f;
        private float _yRotation = 0f;
        private Vector2 _movingDirection = Vector2.zero;
        private Rigidbody _body;
        private View _view = View.FirstPerson;
        private const float CameraOffset = 5f;

        public float sensitivity = 2f;
        public float maxSpeed = 5f;
        public LayerMask playerMask;
        #endregion

        #region Unity logic

        private void OnValidate()
        {
            if (playerMask == default)
                throw new UnassignedReferenceException("Player layer not assigned");
            if (!head)
                throw new UnassignedReferenceException("Head object not assigned");
        }

        private void OnEnable()
        {
            lookingAction.action.performed += context => Look(context.ReadValue<Vector2>());
            movingAction.action.performed += context => _movingDirection = context.ReadValue<Vector2>();
            movingAction.action.canceled += _ => _movingDirection = Vector2.zero;
            switchCamera.action.performed += _ => SwitchCamera();
            movingAction.action.Enable();
            lookingAction.action.Enable();
            switchCamera.action.Enable();
            Cursor.lockState = CursorLockMode.Locked;

            var mRigidBody = GetComponent<Rigidbody>();
            mRigidBody.constraints = RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
            
            var layerIndex = UnityUtils.LayerMaskToLayer(playerMask);
            gameObject.layer = layerIndex;
            head.layer = layerIndex;
            foreach (var child in transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = layerIndex;
            }
            InitEye();
        }

        private void OnDestroy()
        {
            movingAction.action.Disable();
            lookingAction.action.Disable();
            switchCamera.action.Enable();
        }

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Move(_movingDirection);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var headPos = head.transform.position;
            Gizmos.DrawSphere(headPos, 0.1f);
            if (_view == View.FirstPerson) return;
            var ray = new Ray(headPos, mainCamera.transform.position - headPos);
            var isHit = Physics.Raycast(ray,
                out var hit,
                CameraOffset,
                mainCamera.cullingMask);
            Gizmos.color = (_view == View.ThirdPersonBack ? Color.blue : Color.green);
            Gizmos.DrawRay(ray);
            if (!isHit) return;
            Gizmos.DrawLine(ray.origin, hit.point);
            Gizmos.DrawSphere(hit.point, 0.1f);
            mainCamera.transform.position = hit.point;
        }
        #endregion

        #region Interactions
        private void Move(Vector2 direction)
        {
            if (direction == Vector2.zero)
            {
                OnIdle?.Invoke();
                speedup.SetBoostTime(Time.time);
                return;
            }

            var mTransform = transform;
            var movement = mTransform.forward * direction.y + mTransform.right * direction.x;
            var bodyPosition = _body.transform.position;
            var speed = speedup.GetBoostTime(Time.time);
            _body.MovePosition(bodyPosition + movement * (speed * maxSpeed * Time.deltaTime));
            OnMoving?.Invoke(speed);
        }

        private void Look(Vector2 pos)
        {
            TurnAround(pos.x);
            _yRotation -= pos.y * sensitivity;
            _yRotation = Mathf.Clamp(_yRotation, MinYRotation, MaxYRotation);
            head.transform.localRotation = Quaternion.Euler(_yRotation, 0f, 0f);
            
            if (_view == View.FirstPerson) return;
            UpdateCameraOffset();
            var headPos = head.transform.position;
            var ray = new Ray(headPos, mainCamera.transform.position - headPos);
            var isHit = Physics.Raycast(ray,
                out var hit,
                CameraOffset,
                mainCamera.cullingMask);
            if (!isHit) return;
            mainCamera.transform.position = hit.point;
        }

        private void TurnAround(float posX)
        {
            var x = posX * sensitivity;
            transform.Rotate(0f, x, 0f);
        }

        private void InitEye()
        {
            mainCamera.transform.SetParent(head.transform);
            SwitchCamera();
        }

        private void UpdateCameraOffset()
        {
            mainCamera.transform.localPosition =
                (_view switch {
                    View.FirstPerson => Vector3.zero,
                    View.ThirdPersonBack => Vector3.back,
                    _ => Vector3.forward
                }) * CameraOffset;
        }

        private void SwitchCamera()
        {
            switch (_view)
            {
                case View.FirstPerson:
                    UnityUtils.RecognizeLayerToCamera(ref mainCamera, gameObject.layer);
                    _view = View.ThirdPersonFront;
                    break;
                case View.ThirdPersonFront:
                    _view = View.ThirdPersonBack;
                    break;
                case View.ThirdPersonBack:
                default:
                    UnityUtils.IgnoreLayerFromCamera(ref mainCamera, gameObject.layer);
                    _view = View.FirstPerson;
                    break;
            }
            UpdateCameraOffset();
            if (_view == View.FirstPerson) return;
            mainCamera.transform.LookAt(head.transform);
        }
        #endregion

        #region Events
        public event Action OnIdle;
        public event Action<float> OnMoving;
        #endregion
    }
}