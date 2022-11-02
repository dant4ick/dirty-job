using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private CircleCollider2D _feetCollider;
        private BoxCollider2D _headCollider;
        private Rigidbody2D _rigidbody;
        
        private const float MoveSpeed = 6f;
        private float _horizontalMovement;


        private static int _groundLayer;

        private void Start()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _feetCollider = gameObject.GetComponent<CircleCollider2D>();
            _headCollider = gameObject.GetComponent<BoxCollider2D>();
            
            _groundLayer = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            if (_horizontalMovement != 0f)
            {
                if (IsGrounded())
                {
                    _rigidbody.velocity = new Vector2(_horizontalMovement * MoveSpeed, _rigidbody.velocity.y);
                }
            }
            else // Stopping when there is no horizontal input on the ground
            {
                _rigidbody.velocity = new Vector2(0f, _rigidbody.velocity.y);
            }
        }

        private bool IsGrounded()
        {
            return Physics2D.BoxCast(_feetCollider.bounds.center, new Vector2(1f, 1f), 0f, Vector2.down, 1f, _groundLayer);
        }
    }
}