using UnityEngine;
using Player.Input;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private GroundMovement groundMovement = new GroundMovement();
        [SerializeField] private AirMovement airMovement = new AirMovement();

        [Header("Camera-relative movement")]
        [Tooltip("Transform kamera (CameraRig atau Camera.main). Kalau kosong, fallback ke world-space.")]
        [SerializeField] private Transform cameraTransform;

        private const float GroundedStickVelocity = -2f;

        private CharacterController _controller;
        private PlayerInput _input;
        private float _verticalVelocity;

        public bool IsGrounded { get; private set; }

        public float VerticalVelocity => _verticalVelocity;

        public Vector3 Velocity { get; private set; }

        public float CurrentSpeed => groundMovement.CurrentSpeed;

        public bool HasMoveInput => _input.MoveInput.sqrMagnitude > 0.0001f;

        public bool IsRunning => groundMovement.IsRunning(_input.MoveInput, _input.RunHeld);

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            _input.JumpPressed += HandleJumpPressed;
        }

        private void OnDisable()
        {
            _input.JumpPressed -= HandleJumpPressed;
        }

        private void Update()
        {
            IsGrounded = _controller.isGrounded;

            Vector3 moveDirection = GetCameraRelativeDirection(_input.MoveInput);
            Vector3 horizontalVelocity;

            if (IsGrounded)
            {
                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = GroundedStickVelocity;
                }

                horizontalVelocity = groundMovement.CalculateVelocity(
                    moveDirection, _input.RunHeld, transform, Time.deltaTime);
            }
            else
            {
                horizontalVelocity = airMovement.CalculateHorizontalVelocity(moveDirection);
                _verticalVelocity = airMovement.ApplyGravity(_verticalVelocity, Time.deltaTime);
            }

            Vector3 finalVelocity = horizontalVelocity + Vector3.up * _verticalVelocity;
            _controller.Move(finalVelocity * Time.deltaTime);

            Velocity = finalVelocity;
        }

        private Vector3 GetCameraRelativeDirection(Vector2 moveInput)
        {
            if (cameraTransform == null)
            {
                return Vector3.ClampMagnitude(new Vector3(moveInput.x, 0f, moveInput.y), 1f);
            }

            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            Vector3 direction = camForward * moveInput.y + camRight * moveInput.x;
            return Vector3.ClampMagnitude(direction, 1f);
        }

        private void HandleJumpPressed()
        {
            if (IsGrounded)
            {
                _verticalVelocity = airMovement.GetJumpVelocity();
            }
        }
    }
}