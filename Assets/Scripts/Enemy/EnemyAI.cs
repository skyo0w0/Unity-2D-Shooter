using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
public class EnemyAI : MonoBehaviour
{
    #region Layer Selection
    [SerializeField] private LayerMask _gameplayLayers;
    [SerializeField] private LayerMask _obstacleLayers;
    #endregion
    
    #region Enemy Attributes
    [SerializeField] private float _speed = 2f;
    #endregion
    
    #region PathFinding Attributes
    private Transform _player;
    private GridManager gridManager;
    private AStarPathfinding pathfinding;
    private List<Vector2Int> currentPath; 
    private int pathIndex = 0;
    #endregion
    
    #region RayCasting
    private Vector2[] directions = {Vector2.left, Vector2.right, Vector2.up, Vector2.down};
    #endregion

    #region Shooting Attributes
    [SerializeField] private float _shootingCheckRadius = 10f;
    [SerializeField] private float _shootCooldown = 1.0f;
    [SerializeField] private ShooterComponent shooterComponent;
    private float _lastShootTime = -Mathf.Infinity;
    private List<Vector2> _shootDir;
    #endregion
    
    #region Zenject
    [Inject]
    private void Construct(PlayerMovementComponent playerMovementComponent)
    {
        _player = playerMovementComponent.transform;
    }
    #endregion

    #region Unity Lifecycle
    private void Start()
    {
        _shootDir = new List<Vector2>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinding = new AStarPathfinding(gridManager);
        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f); 
    }
    
    private void Update()
    {
        if (Time.time >= _lastShootTime + _shootCooldown && CanShoot())
        {
            Shoot();
        }

        if (currentPath == null || pathIndex >= currentPath.Count) return;

        Vector2 targetPosition = gridManager.GridToWorldPosition(currentPath[pathIndex]);
        
        transform.position = 
            Vector2.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);
        
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            pathIndex++;
        }
    }
    #endregion
    
    #region Pathfinding
    private void UpdatePath()
    {

        Vector2Int startGrid = gridManager.WorldToGridPosition(transform.position);
        Vector2Int targetGrid = gridManager.WorldToGridPosition(_player.position);
        
        currentPath = pathfinding.FindPath(startGrid, targetGrid);
        pathIndex = 0;
    }
    #endregion

    #region Shooting Methods
    private void Shoot()
    {
        _lastShootTime = Time.time;
        int attackIndex = Random.Range(0,PossibleTargets().Count);
        Vector2 attDir = _shootDir[attackIndex];
        float angle = Mathf.Atan2(attDir.y, attDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        shooterComponent.ShootBullet();
        
    }

    private bool CanShoot()
    {
        _shootDir.Clear();
        _shootDir = PossibleTargets();
        
        if (_shootDir.Count == 0) return false;
        
        return true;
    }

    private List<Vector2> PossibleTargets()
    {
        Vector2 target = _player.position;
        Vector2 direction = ( target - new Vector2(transform.position.x,transform.position.y)).normalized;
        List<Vector2> possibleTargets = new List<Vector2>();
        
        if (Vector2.Distance(target,transform.position) < _shootingCheckRadius)
        {
            RaycastHit2D[] hits =
                Physics2D.RaycastAll(transform.position, direction, _shootingCheckRadius, _gameplayLayers);
            Debug.DrawRay(transform.position, direction * _shootingCheckRadius, Color.red);
            if (hits.Length == 1)
            {
                possibleTargets.Add(direction);
            }

            possibleTargets.AddRange(CheckCellsAroundTarget());
        }
        
        
        return possibleTargets;
    }

    private List<Vector2> CheckCellsAroundTarget()
    {
        Vector2 target = _player.position;
        List<Vector2> cells = new List<Vector2>();
        
        foreach (Vector2 dir in directions)
        {
          RaycastHit2D[] hits = 
              Physics2D.RaycastAll(target,dir, _shootingCheckRadius/2f ,_obstacleLayers);
          Debug.DrawRay(target,dir * _shootingCheckRadius/2f, Color.white);
          if ((hits.Length == 1) && (((1 << hits[0].collider.gameObject.layer) & _obstacleLayers) != 0))
          {
              cells.Add(hits[0].point);
          }
        }

        List<Vector2> possibleDirs = new List<Vector2>();
        
        foreach (Vector2 point in cells)
        {
            Vector2 direction = (point - new Vector2(transform.position.x,transform.position.y)).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, _shootingCheckRadius, _obstacleLayers);
            Debug.DrawRay(transform.position,direction * _shootingCheckRadius,Color.yellow);
            if (hit.point == point)
            {
                possibleDirs.Add(direction);
            }
        }
        
        return possibleDirs;
        
    }
    
    #endregion
    
    #region Debug
    private void OnDrawGizmos()
    {
        if (currentPath == null || gridManager == null) return;

        Gizmos.color = Color.blue;

        foreach (Vector2Int node in currentPath)
        {
            Vector2 worldPosition = gridManager.GridToWorldPosition(node); 
            Gizmos.DrawSphere(worldPosition, gridManager.cellSize / 4);
        }

        foreach (Vector2 point in _shootDir)
        {
            Gizmos.DrawSphere(point, 0.16f);
        }
    }
    
    #endregion
}
