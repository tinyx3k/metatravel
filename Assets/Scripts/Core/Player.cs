using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Camera eye;
        [SerializeField] private InputActionReference movingAction;
        [SerializeField] private InputActionReference lookingAction;
        private readonly float _minYRotation = -90f;
        private readonly float _maxYRotation = 90f;
        private float _xRotation = 0f;
        private Vector2 _movingDirection = Vector2.zero;
        private Rigidbody _body;
        public float sensitivity = 2f;
        public float speed = 2f;

        private void OnEnable()
        {
            lookingAction.action.performed += context => Look(context.ReadValue<Vector2>());
            movingAction.action.performed += context => _movingDirection = context.ReadValue<Vector2>();
            movingAction.action.canceled += _ => _movingDirection = Vector2.zero;
            movingAction.action.Enable();
            lookingAction.action.Enable();
        }        
        private void OnDestroy()
        {
            movingAction.action.Disable();
            lookingAction.action.Disable();
        }

        private void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Move(_movingDirection);
        }

        private void Move(Vector2 direction)
        {
            var bodyTransform = transform;
            var movement = bodyTransform.forward * direction.y + bodyTransform.right * direction.x;
            _body.MovePosition(bodyTransform.position + movement * (speed * Time.deltaTime));
        }

        private void Look(Vector2 pos)
        {
            TurnAround(pos.x);
            _xRotation -= pos.y * sensitivity;
            _xRotation = Mathf.Clamp(_xRotation, _minYRotation, _maxYRotation);
            eye.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }
        private void TurnAround(float posX)
        {
            var x = posX * sensitivity;
            transform.Rotate(0f, x, 0f);
        }
    }   
}