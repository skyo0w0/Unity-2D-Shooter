using BulletSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(LineRenderer))]
    public class PlayerLineRendererComponent : MonoBehaviour
    {
        private Vector2 _mousePosition;
        private LineRenderer _lineRenderer;

        [FormerlySerializedAs("objectsLayer")] [SerializeField]
        private LayerMask obstacleLayer; 

        [SerializeField] private int maxReflections = 3; 
        [SerializeField] private float maxDistance = 10f; 
        [SerializeField] private Transform bulletSpawn;
        private LineTrajectory _lineTrajectory;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineTrajectory = new LineTrajectory(_lineRenderer, obstacleLayer, maxReflections, maxDistance);
        }


        private void Update()
        {
            UpdateLineRenderer(bulletSpawn.position);
        }


        private void UpdateLineRenderer(Vector2 mousePosition)
        {
            _lineTrajectory.CalculateTrajectory(transform.position, mousePosition);
        }

    }
}