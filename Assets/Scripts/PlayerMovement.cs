using System;
using Data;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerMovement : MonoBehaviour {
    [Header("Movement")] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    [Header("View")] [SerializeField] private Camera eyesCamera;
    [Range(0, 1)] [SerializeField] private float rotateSpeed = 0.5f;
    [SerializeField] private FloatRange viewXRange = new(-80, 80);


    private InputAction _movement;
    private InputAction _look;
    private InputAction _jump;

    private Rigidbody _rigidbody;
    private float _currentRotationX = 0.0f;
    private bool _jumpTriggered = false;

    void Awake() {
        //Component rigidbody is deprecated
        _rigidbody = GetComponent<Rigidbody>();
        _movement = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _jump = InputSystem.actions.FindAction("Jump");

        if (eyesCamera == null) {
            eyesCamera = Camera.main;
        }
    }


    void Update() {
        LookRotate();

        if (_jump.triggered && IsGrounded()) {
            _jumpTriggered = true;
        }
    }

    private void FixedUpdate() {
        Movement();
    }


    void Movement() {
        var moveInput = _movement.ReadValue<Vector2>();
        var newVelocity = transform.forward * (moveInput.y * moveSpeed) +
                          transform.right * (moveInput.x * moveSpeed);

        if (_jumpTriggered) {
            //jump
            _jumpTriggered = false;
            newVelocity.y = jumpForce;
        }
        else {
            newVelocity.y = _rigidbody.linearVelocity.y;
        }
        
        _rigidbody.linearVelocity = newVelocity;
    }


    bool IsGrounded() {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    void LookRotate() {
        var lookInput = _look.ReadValue<Vector2>();
        transform.Rotate(lookInput.x * rotateSpeed * Vector3.up);
        var lookInputY = lookInput.y * rotateSpeed;
        _currentRotationX -= lookInputY;
        _currentRotationX = Mathf.Clamp(_currentRotationX, viewXRange.Min, viewXRange.Max);
        eyesCamera.transform.localEulerAngles = new Vector3(_currentRotationX, 0.0f, 0.0f);
    }
}