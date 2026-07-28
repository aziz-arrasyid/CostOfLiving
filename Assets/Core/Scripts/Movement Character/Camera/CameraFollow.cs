using UnityEngine;

namespace Player.CameraSystem
{
    [DefaultExecutionOrder(-100)] 
    public class CameraFollow : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Transform target; 

        [Header("Offset & Smoothing")]
        [SerializeField] private Vector3 offset = new Vector3(0f, 1.6f, 0f); 
        [SerializeField] private float followSmoothTime = 0.08f;

        private Vector3 _velocity;

        private void LateUpdate()
        {
            if (target == null) return;

            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(
                transform.position, desiredPosition, ref _velocity, followSmoothTime);
        }
    }
}
