using UnityEngine;


public class GridManager : MonoBehaviour
{
    public Vector2 gridSize = new Vector2(20, 10); // Размеры сетки в мировых единицах
    public float cellSize = 1f; // Размер одной ячейки
    public LayerMask obstacleLayer; // Слой для препятствий

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;

        for (float x = 0; x < gridSize.x; x += cellSize)
        {
            for (float y = 0; y < gridSize.y; y += cellSize)
            {
                Vector2 worldPosition = new Vector2(transform.position.x + x, transform.position.y + y);

                // Определяем проходимость ячейки
                if (IsCellWalkable(worldPosition))
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireCube(worldPosition, Vector3.one * cellSize);
            }
        }
    }
    
    public bool IsCellWalkable(Vector2 position)
    {
        return !Physics2D.OverlapCircle(position, cellSize / 4, obstacleLayer);
    }
    
    public Vector2Int WorldToGridPosition(Vector2 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / cellSize),
            Mathf.FloorToInt(worldPosition.y / cellSize)
        );
    }

    public Vector2 GridToWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(
            gridPosition.x * cellSize + cellSize / 2,
            gridPosition.y * cellSize + cellSize / 2
        );
    }
}