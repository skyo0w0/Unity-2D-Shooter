using BulletSystem;
using UnityEngine;

public class ShooterComponent : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    private BulletManager _bulletManager;
    private Vector2 _mousePosition;
    private LineRenderer _lineRenderer;
    

    private void Awake()
    {
        _bulletManager = new BulletManager(bulletPrefab);
    }
    
    
    public void ShootBullet()
    {
        _bulletManager.Shoot(bulletSpawn.position, (bulletSpawn.position - transform.position).normalized);
    }
}
