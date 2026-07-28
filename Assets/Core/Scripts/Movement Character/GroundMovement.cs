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

        public Vector3 CalculateVelocity(Vector3 moveDirection, bool runHeld, Transform playerTransform, float deltaTime)
        {
            bool isMoving = moveDirection.sqrMagnitude > 0.0001f;
            CurrentSpeed = isMoving ? (runHeld ? runSpeed : walkSpeed) : 0f;

            Vector3 horizontalVelocity = moveDirection * CurrentSpeed;

            if (isMoving)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
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