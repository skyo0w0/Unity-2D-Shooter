using UnityEngine.Serialization;

namespace BulletSystem
{
    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f; 
        [SerializeField] private LayerMask _bounceLayer;
        [SerializeField] private int _maxBounces = 1;
        private int _bounces = 0;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Initialize(Vector2 initialDirection)
        {

            _rigidbody2D.velocity = initialDirection.normalized * _speed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_bounces >= 3 || collision.gameObject.layer == LayerMask.NameToLayer("WorldBound"))
            {
                Destroy(gameObject);
            }
            else if (((1 << collision.gameObject.layer) & _bounceLayer) != 0)
            {

                Vector2 normal = collision.contacts[0].normal;


                _rigidbody2D.velocity = normal.normalized * _speed;
                
                _bounces++;

            }
        }
    }
}