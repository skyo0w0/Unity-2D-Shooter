using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class LineTrajectory
    {
        private LineRenderer _lineRenderer;
        private LayerMask _objectsLayer;
        private int _maxReflections;
        private float _maxDistance;

        public LineTrajectory(LineRenderer lineRenderer, LayerMask objectsLayer, int maxReflections, float maxDistance)
        {
            _lineRenderer = lineRenderer;
            _objectsLayer = objectsLayer;
            _maxReflections = maxReflections;
            _maxDistance = maxDistance;
        }

        public List<Vector2> CalculateTrajectory(Vector2 startPosition, Vector2 targetPosition)
        {
            Vector2 direction = (targetPosition - startPosition).normalized;
            List<Vector2> trajectoryPoints = new List<Vector2> { startPosition };
            HashSet<Collider2D> processedColliders = new HashSet<Collider2D>();
            float remainingDistance = _maxDistance;

            for (int i = 0; i < _maxReflections; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, remainingDistance, _objectsLayer);
                Debug.DrawRay(hit.point, Vector2.zero, Color.yellow, 1f);
                
                if (hit.collider != null)
                {
                    processedColliders.Add(hit.collider);
                    // Добавляем точку столкновения
                    trajectoryPoints.Add(hit.point);

                    // Устанавливаем новое направление (перпендикулярное к поверхности)
                    direction = hit.normal.normalized;

                    // Уменьшаем оставшуюся длину на пройденное расстояние
                    remainingDistance -= hit.distance;

                    // Обновляем начальную точку для следующего отражения
                    startPosition = hit.point + hit.normal * 0.01f;

                    // Визуализация нового направления
                    Debug.DrawRay(hit.point, direction * 2, Color.green, 1f);


                }
                else
                {
                    trajectoryPoints.Add(startPosition + direction * remainingDistance);
                    break;
                }
            }

            // Устанавливаем точки для LineRenderer
            _lineRenderer.positionCount = trajectoryPoints.Count;
            _lineRenderer.SetPositions(trajectoryPoints.ConvertAll(point => (Vector3)point).ToArray());

            return trajectoryPoints;
        }
        
    }
}

