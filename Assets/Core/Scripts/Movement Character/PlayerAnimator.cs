using UnityEngine;

namespace Player.Animation
{
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("Animator (isi belakangan, kalau model + Animator Controller sudah ada)")]
        [SerializeField] private Animator animator;
        [SerializeField] private float crossFadeDuration = 0.1f;

        [Header("Debug Visual (isi sekarang, pakai Renderer dari dummy capsule/sphere)")]
        [SerializeField] private Renderer debugRenderer;
        [SerializeField] private Color idleColor = Color.white;
        [SerializeField] private Color walkColor = Color.yellow;
        [SerializeField] private Color runColor = new Color(1f, 0.5f, 0f); 
        [SerializeField] private Color jumpColor = Color.cyan;
        [SerializeField] private Color fallColor = Color.red;

        public void SetState(PlayerAnimState state)
        {
            ApplyDebugColor(state);
            ApplyAnimatorState(state);
        }

        private void ApplyDebugColor(PlayerAnimState state)
        {
            if (debugRenderer == null) return;

            Color color = state switch
            {
                PlayerAnimState.Idle => idleColor,
                PlayerAnimState.Walk => walkColor,
                PlayerAnimState.Run => runColor,
                PlayerAnimState.Jump => jumpColor,
                PlayerAnimState.Fall => fallColor,
                _ => Color.magenta
            };

            debugRenderer.material.color = color;
        }

        private void ApplyAnimatorState(PlayerAnimState state)
        {
            if (animator == null) return;

            animator.CrossFade(state.ToString(), crossFadeDuration);
        }
    }
}