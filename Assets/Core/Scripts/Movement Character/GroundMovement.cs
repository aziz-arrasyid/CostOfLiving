using UnityEngine;

namespace Player.Movement
{
    [System.Serializable]
    public class GroundMovement
    {
        [Header("Speed")]
        [SerializeField] private float walkSpeed = 3f;
        [SerializeField] private float runSpeed = 6f;

        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 12f;

        public float CurrentSpeed { get; private set; }

        public Vector3 CalculateVelocity(Vector2 moveInput, bool runHeld, Transform playerTransform, float deltaTime)
        {
            Vector3 inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            inputDirection = Vector3.ClampMagnitude(inputDirection, 1f);

            bool isMoving = inputDirection.sqrMagnitude > 0.0001f;
            CurrentSpeed = isMoving ? (runHeld ? runSpeed : walkSpeed) : 0f;

            Vector3 horizontalVelocity = inputDirection * CurrentSpeed;

            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection, Vector3.up);
                playerTransform.rotation = Quaternion.Slerp(
                    playerTransform.rotation,
                    targetRotation,
                    rotationSpeed * deltaTime
                );
            }

            return horizontalVelocity;
        }

        public bool IsMoving(Vector2 moveInput) => moveInput.sqrMagnitude > 0.0001f;

        public bool IsRunning(Vector2 moveInput, bool runHeld) => IsMoving(moveInput) && runHeld;
    }
}
