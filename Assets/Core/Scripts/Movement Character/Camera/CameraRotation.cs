using UnityEngine;
using UnityEngine.InputSystem;
using Player.Input;

namespace Player.CameraSystem
{
    [DefaultExecutionOrder(-50)] 
    public class CameraRotation : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("PlayerInput milik karakter, sumber LookInput.")]
        [SerializeField] private Player.Input.PlayerInput playerInput;

        [Header("Sensitivity")]
        [SerializeField] private float yawSpeed = 200f;
        [SerializeField] private float pitchSpeed = 150f;

        [Header("Pitch Clamp")]
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 60f;

        [Header("Free Cursor (hold Alt)")]
        [Tooltip("Kalau dimatikan, fitur hold-Alt-buat-cursor ini nonaktif total.")]
        [SerializeField] private bool enableFreeCursor = true;

        private float _yaw;
        private float _pitch;
        private bool _wasAltHeld;

        private void Start()
        {
            Vector3 currentEuler = transform.eulerAngles;
            _yaw = currentEuler.y;
            _pitch = currentEuler.x;

            LockCursor();
        }

        private void LateUpdate()
        {
            if (playerInput == null) return;

            bool altHeld = enableFreeCursor && Keyboard.current != null && Keyboard.current.leftAltKey.isPressed;

            if (altHeld && !_wasAltHeld)
            {
                FreeCursor();
            }
            else if (!altHeld && _wasAltHeld)
            {
                LockCursor();
            }

            _wasAltHeld = altHeld;

            if (altHeld) return;

            Vector2 look = playerInput.LookInput;

            _yaw += look.x * yawSpeed * Time.deltaTime;
            _pitch -= look.y * pitchSpeed * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }

        private void FreeCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (Mouse.current != null)
            {
                Mouse.current.WarpCursorPosition(new Vector2(Screen.width * 0.5f, Screen.height * 0.5f));
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}