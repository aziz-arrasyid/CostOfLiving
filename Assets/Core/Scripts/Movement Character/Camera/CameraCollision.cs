using UnityEngine;

namespace Player.CameraSystem
{
    [DefaultExecutionOrder(0)] 
    public class CameraCollision : MonoBehaviour
    {
        [Header("Pivot")]
        [Tooltip("CameraRig / pivot tempat kamera ini jadi child-nya.")]
        [SerializeField] private Transform pivot;

        [Header("Distance")]
        [SerializeField] private float desiredDistance = 4f;
        [SerializeField] private float minDistance = 0.5f;
        [SerializeField] private float collisionRadius = 0.2f;
        [SerializeField] private LayerMask obstacleMask = ~0; 

        [Header("Smoothing")]
        [SerializeField] private float distanceSmoothTime = 0.05f;

        private float _currentDistance;
        private float _distanceVelocity;

        private void Start()
        {
            _currentDistance = desiredDistance;
        }

        private void LateUpdate()
        {
            if (pivot == null) return;

            Vector3 direction = -pivot.forward;
            float targetDistance = desiredDistance;

            if (Physics.SphereCast(
                    pivot.position, collisionRadius, direction,
                    out RaycastHit hit, desiredDistance, obstacleMask))
            {
                targetDistance = Mathf.Max(hit.distance, minDistance);
            }

            _currentDistance = Mathf.SmoothDamp(
                _currentDistance, targetDistance, ref _distanceVelocity, distanceSmoothTime);

            transform.localPosition = new Vector3(0f, 0f, -_currentDistance);
        }
    }
}
