namespace Player
{
    using UnityEngine;
    using Zenject;

    using UnityEngine;
    using Zenject;

    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementComponent : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;

        private InputHandler _inputHandler;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _movementInput;
        private Vector2 _mousePosition;

        [Inject]
        public void Construct(InputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _inputHandler.OnRight += () => UpdateMovement(Vector2.right);
            _inputHandler.OnLeft += () => UpdateMovement(Vector2.left);
            _inputHandler.OnHorizontalReleased += () => UpdateMovement(Vector2.zero, axis: "x");
            _inputHandler.OnUp += () => UpdateMovement(Vector2.up);
            _inputHandler.OnDown += () => UpdateMovement(Vector2.down);
            _inputHandler.OnVerticalReleased += () => UpdateMovement(Vector2.zero, axis: "y");
            _inputHandler.OnMouseMove += UpdateMousePosition;
        }

        private void OnDisable()
        {
            _inputHandler.OnRight -= () => UpdateMovement(Vector2.right);
            _inputHandler.OnLeft -= () => UpdateMovement(Vector2.left);
            _inputHandler.OnHorizontalReleased -= () => UpdateMovement(Vector2.zero, axis: "x");
            _inputHandler.OnUp -= () => UpdateMovement(Vector2.up);
            _inputHandler.OnDown -= () => UpdateMovement(Vector2.down);
            _inputHandler.OnVerticalReleased -= () => UpdateMovement(Vector2.zero, axis: "y");
            _inputHandler.OnMouseMove -= UpdateMousePosition;
        }

        private void FixedUpdate()
        {
            Vector2 normalizedInput = _movementInput.normalized;
            _rigidbody2D.velocity = normalizedInput * moveSpeed;
            
            RotateTowardsMouse();
        }

        private void UpdateMovement(Vector2 direction, string axis = "")
        {
            if (axis == "x")
            {
                _movementInput.x = direction.x;
            }
            else if (axis == "y")
            {
                _movementInput.y = direction.y;
            }
            else
            {
                _movementInput += direction;
            }
        }

        private void UpdateMousePosition(Vector2 mousePosition)
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main Camera is not assigned in the scene!");
                return;
            }

            _mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        }

        private void RotateTowardsMouse()
        {
            Vector2 direction = _mousePosition - (Vector2)transform.position;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
