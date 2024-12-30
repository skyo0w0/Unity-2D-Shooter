using System.Collections.Generic;
using BulletSystem;
using UnityEngine;

namespace BulletSystem
{
    using UnityEngine;

    public class BulletManager
    {
        private GameObject _bulletPrefab;
        public BulletManager(GameObject bulletPrefab)
        {
            _bulletPrefab = bulletPrefab;
        }
        
        public void Shoot(Vector2 startPosition, Vector2 initialDirection)
        {
            if (_bulletPrefab == null)
            {
                Debug.LogError("Bullet prefab is not assigned!");
                return;
            }
            
            GameObject bullet = Object.Instantiate(_bulletPrefab, startPosition, Quaternion.identity);
            
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.Initialize(initialDirection);
            }
            else
            {
                Debug.LogError("Bullet script is missing on the bullet prefab!");
                Object.Destroy(bullet); 
            }
        }
    }
}
