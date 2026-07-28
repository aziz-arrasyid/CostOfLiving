using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Input
{
    [DisallowMultipleComponent]
    public class PlayerInput : MonoBehaviour
    {
        private PlayerInputActions _inputActions;

        public Vector2 MoveInput { get; private set; }

        public bool RunHeld { get; private set; }

        public event Action JumpPressed;

        private void Awake()
        {
            _inputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _inputActions.Player.Enable();

            _inputActions.Player.Move.performed += OnMovePerformed;
            _inputActions.Player.Move.canceled += OnMoveCanceled;

            _inputActions.Player.Run.performed += OnRunPerformed;
            _inputActions.Player.Run.canceled += OnRunCanceled;

            _inputActions.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            _inputActions.Player.Move.performed -= OnMovePerformed;
            _inputActions.Player.Move.canceled -= OnMoveCanceled;

            _inputActions.Player.Run.performed -= OnRunPerformed;
            _inputActions.Player.Run.canceled -= OnRunCanceled;

            _inputActions.Player.Jump.performed -= OnJumpPerformed;

            _inputActions.Player.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext ctx) => MoveInput = ctx.ReadValue<Vector2>();
        private void OnMoveCanceled(InputAction.CallbackContext ctx) => MoveInput = Vector2.zero;

        private void OnRunPerformed(InputAction.CallbackContext ctx) => RunHeld = true;
        private void OnRunCanceled(InputAction.CallbackContext ctx) => RunHeld = false;

        private void OnJumpPerformed(InputAction.CallbackContext ctx) => JumpPressed?.Invoke();
    }
}
