using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding
{
    #region Pathfinding Attributes
    private GridManager gridManager;
    private List<APathNode> openList;
    private HashSet<Vector2Int> closedList;
    #endregion
    public AStarPathfinding(GridManager grid)
    {
        gridManager = grid;
    }

    public List<Vector2Int> FindPath(Vector2 start, Vector2 target)
    {
        Vector2Int startGrid = gridManager.WorldToGridPosition(start);
        Vector2Int targetGrid = gridManager.WorldToGridPosition(target);

        openList = new List<APathNode> { new APathNode(startGrid) };
        closedList = new HashSet<Vector2Int>();

        while (openList.Count > 0)
        {
            APathNode currentNode = GetLowestFCostNode();
            
            if (currentNode.Position == targetGrid)
            {
                return ReconstructPath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode.Position);

            foreach (Vector2Int neighbor in GetNeighbors(currentNode.Position))
            {
                if (closedList.Contains(neighbor) || !gridManager.IsCellWalkable(gridManager.GridToWorldPosition(neighbor)))
                    continue;

                APathNode neighborNode = new APathNode(neighbor)
                {
                    Parent = currentNode
                };
                
                neighborNode.CalculateCosts(targetGrid, 10);
                APathNode existingNode = openList.Find(n => n.Position == neighbor);
                
                if (existingNode != null && existingNode.GCost <= neighborNode.GCost)
                    continue;

                openList.Add(neighborNode);
            }
        }

        return new List<Vector2Int>(); // Если путь не найден
    }

    private APathNode GetLowestFCostNode()
    {
        APathNode lowestNode = openList[0];
        foreach (APathNode node in openList)
        {
            if (node.FCost < lowestNode.FCost || (node.FCost == lowestNode.FCost && node.HCost < lowestNode.HCost))
                lowestNode = node;
        }
        return lowestNode;
    }

    private List<Vector2Int> ReconstructPath(APathNode node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }
        path.Reverse();
        return path;
    }

    private List<Vector2Int> GetNeighbors(Vector2Int position)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            position + Vector2Int.up,
            position + Vector2Int.down,
            position + Vector2Int.left,
            position + Vector2Int.right
        };

        return neighbors;
    }
}


public class APathNode
{
    public Vector2Int Position;
    public int GCost; // Стоимость пути от старта до текущей клетки
    public int HCost; // Оценка расстояния до цели
    public int FCost => GCost + HCost; // Полная стоимость
    public APathNode Parent;

    public APathNode(Vector2Int position)
    {
        Position = position;
    }

    public void CalculateCosts(Vector2Int targetPosition, int movementCost)
    {
        GCost = Parent != null ? Parent.GCost + movementCost : 0;
        HCost = Mathf.Abs(targetPosition.x - Position.x) + Mathf.Abs(targetPosition.y - Position.y);
    }
}
