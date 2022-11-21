using System;
using Item.Weapon;
using UnityEngine;
using UnityEngine.UIElements;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private static PlayerController _instance;

        public static PlayerController Instance
        {
            get { return _instance; }
        }

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _rigidbody;

        private const float MoveSpeed = 6f;
        private float _horizontalMovement;

        private static int _groundLayer;

        [Header("Friction materials")] 
        [SerializeField] private PhysicsMaterial2D zeroFriction;
        [SerializeField] private PhysicsMaterial2D maxFriction;

        private void Start()
        {
            _instance = this;

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _playerCollider = gameObject.GetComponent<CapsuleCollider2D>();

            _groundLayer = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        private void FixedUpdate()
        {
            if (!IsGrounded()) return;

            if (_horizontalMovement == 0f)
            {
                _rigidbody.sharedMaterial = maxFriction;
                return;
            }

            _rigidbody.sharedMaterial = zeroFriction;
            _rigidbody.velocity = new Vector2(_horizontalMovement * MoveSpeed, _rigidbody.velocity.y);
        }

        private bool IsGrounded()
        {
            var bounds = _playerCollider.bounds;
            var radius = bounds.size.x * .4f;
            var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

            var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _groundLayer);

            Debug.DrawRay(hitPoint.point, hitPoint.normal);

            return hitPoint;
        }
    }
}