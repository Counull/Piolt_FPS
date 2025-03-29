using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour {
    private InputAction _movement;
    private InputAction _look;
    private InputAction _jump;
    private CharacterController _controller;

    void Awake() {
        _movement = InputSystem.actions.FindAction("Move");
        _look = InputSystem.actions.FindAction("Look");
        _jump = InputSystem.actions.FindAction("Jump");
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update() { }
}