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

            Vector3 horizontalVelocity;

            if (IsGrounded)
            {
                if (_verticalVelocity < 0f)
                {
                    _verticalVelocity = GroundedStickVelocity;
                }

                horizontalVelocity = groundMovement.CalculateVelocity(
                    _input.MoveInput, _input.RunHeld, transform, Time.deltaTime);
            }
            else
            {
                horizontalVelocity = airMovement.CalculateHorizontalVelocity(_input.MoveInput);
                _verticalVelocity = airMovement.ApplyGravity(_verticalVelocity, Time.deltaTime);
            }

            Vector3 finalVelocity = horizontalVelocity + Vector3.up * _verticalVelocity;
            _controller.Move(finalVelocity * Time.deltaTime);

            Velocity = finalVelocity;
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