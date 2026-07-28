using UnityEngine;

namespace Player.Movement
{
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

        public Vector3 CalculateHorizontalVelocity(Vector2 moveInput)
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            direction = Vector3.ClampMagnitude(direction, 1f);

            return direction * airSpeed;
        }

        public bool IsAscending(float verticalVelocity) => verticalVelocity > 0f;

        public bool IsFalling(float verticalVelocity) => verticalVelocity <= 0f;
    }
}