using UnityEngine;
using Player.Input;

namespace Player.CameraSystem
{
    [DefaultExecutionOrder(-50)] 
    public class CameraRotation : MonoBehaviour
    {
        [Header("Target")]
        [Tooltip("PlayerInput milik karakter, sumber LookInput.")]
        [SerializeField] private PlayerInput playerInput;

        [Header("Sensitivity")]
        [SerializeField] private float yawSpeed = 200f;
        [SerializeField] private float pitchSpeed = 150f;

        [Header("Pitch Clamp")]
        [SerializeField] private float minPitch = -30f;
        [SerializeField] private float maxPitch = 60f;

        private float _yaw;
        private float _pitch;

        private void Start()
        {
            Vector3 currentEuler = transform.eulerAngles;
            _yaw = currentEuler.y;
            _pitch = currentEuler.x;

           
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (playerInput == null) return;

            Vector2 look = playerInput.LookInput;

            _yaw += look.x * yawSpeed * Time.deltaTime;
            _pitch -= look.y * pitchSpeed * Time.deltaTime;
            _pitch = Mathf.Clamp(_pitch, minPitch, maxPitch);

            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
    }
}
