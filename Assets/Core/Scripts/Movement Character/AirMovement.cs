using UnityEngine;

namespace Player.Movement
{
    /// <summary>
    /// Layer "Movement System" (khusus airborne) dari alur:
    /// PlayerInput -> [AirMovement] -> PlayerMotor (Physics)
    ///
    /// Tugasnya menghitung:
    /// 1. Velocity vertikal (gravity saat naik/turun, jump force saat mulai lompat)
    /// 2. Velocity horizontal terbatas saat di udara (air control)
    ///
    /// Tidak menyentuh CharacterController sama sekali — itu tugas PlayerMotor.
    /// </summary>
    [System.Serializable]
    public class AirMovement
    {
        [Header("Gravity")]
        [SerializeField] private float gravity = -20f;
        [SerializeField] private float terminalVelocity = -25f;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 8f;

        [Header("Air Control")]
        [SerializeField] private float airSpeed = 4f;

        public float ApplyGravity(float currentVerticalVelocity, float deltaTime)
        {
            float newVerticalVelocity = currentVerticalVelocity + gravity * deltaTime;
            return Mathf.Max(newVerticalVelocity, terminalVelocity);
        }

        public float GetJumpVelocity() => jumpForce;

        public Vector3 CalculateHorizontalVelocity(Vector3 moveDirection)
        {
            return moveDirection * airSpeed;
        }

        public bool IsAscending(float verticalVelocity) => verticalVelocity > 0f;

        public bool IsFalling(float verticalVelocity) => verticalVelocity <= 0f;
    }
}