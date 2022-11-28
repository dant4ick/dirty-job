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

        private Pivot _handPivot;

        [Header("Objects for pivot")]
        [SerializeField] private GameObject hand;
        [SerializeField] private Camera mainCamera;

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _rigidbody;

        private const float MoveSpeed = 6f;
        private float _horizontalMovement;

        [SerializeField] private float slopeCheckDistance;
        private float _slopeDownAngle;
        private float _slopeDownAngleOld;
        private float _slopeSideAngle;
        private Vector2 _slopeNormalPerp;
        private bool _isOnGround;
        private bool _isOnSlope;
        private bool _isOnPlatform;

        private static int _groundLayer;
        private static int _platformLayer;

        [SerializeField] private Animator animator;
        private static readonly int CharacterIdle = Animator.StringToHash("Character_Idle");
        private static readonly int CharacterIdleNoHand = Animator.StringToHash("Character_IdleNoHand");
        private static readonly int CharacterRun = Animator.StringToHash("Character_Run");
        private static readonly int CharacterAttackBat = Animator.StringToHash("Character_AttackBat");
        private static readonly int CharacterAttackKnife = Animator.StringToHash("Character_AttackKnife");
        private int _currentAnimation;
        private bool _isFacingRight;

        [Header("Friction materials")] 
        [SerializeField] private PhysicsMaterial2D zeroFriction;
        [SerializeField] private PhysicsMaterial2D maxFriction;

        private void Start()
        {
            _currentAnimation = CharacterIdle;
            _isFacingRight = false;

            _instance = this;
            _handPivot = hand.GetComponent<Pivot>();

            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            _playerCollider = gameObject.GetComponent<CapsuleCollider2D>();

            _groundLayer = LayerMask.GetMask("Ground");
            _platformLayer = LayerMask.GetMask("Platform");
            
        }

        private void Update()
        {
            _horizontalMovement = Input.GetAxisRaw("Horizontal");

            gameObject.layer = LayerMask.NameToLayer(Input.GetKey(KeyCode.S) ? "PlayerThroughPlatform" : "Player");

            // Animation manager
            if (Input.GetAxisRaw("Horizontal") < 0f && !_isFacingRight)
            {
                Flip();
            }
            if (Input.GetAxisRaw("Horizontal") > 0f && _isFacingRight)
            {
                Flip();
            }
            if (_isOnGround)
            {
                ChangeAnimationState((_horizontalMovement != 0f) ? CharacterRun : CharacterIdle);
            }
        }
        
        private void Flip()
        {
            _isFacingRight = !_isFacingRight;
            transform.localScale = new Vector3(transform.localScale.x * -1f, 1f, 1f);
        }
        
        private void ChangeAnimationState(int newAnimation)
        {
            if (newAnimation == _currentAnimation) return;
            animator.Play(newAnimation);
            _currentAnimation = newAnimation;
        }

        private void FixedUpdate()
        {
            _handPivot.ChangeRotation(mainCamera.ScreenToWorldPoint(Input.mousePosition));

            GroundCheck();
            SlopeCheck();
            PlatformCheck();

            if (_horizontalMovement == 0f)
            {
                _rigidbody.sharedMaterial = maxFriction;
                return;
            }

            if (_isOnGround || _isOnPlatform)
            {
                Move();
            }            
        }

        private void Move()
        {
            _rigidbody.sharedMaterial = zeroFriction;

            if (_isOnSlope)
                _rigidbody.velocity = new Vector2(-_horizontalMovement * MoveSpeed * _slopeNormalPerp.x, -_horizontalMovement * MoveSpeed * _slopeNormalPerp.y);
            else
                _rigidbody.velocity = new Vector2(_horizontalMovement * MoveSpeed, _rigidbody.velocity.y);

        }

        private void GroundCheck()
        {
            var bounds = _playerCollider.bounds;
            var radius = bounds.size.x * .4f;
            var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

            var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _groundLayer);

            Debug.DrawRay(hitPoint.point, hitPoint.normal);

            _isOnGround = hitPoint;
        }

        private void PlatformCheck()
        {
            var bounds = _playerCollider.bounds;
            var radius = bounds.size.x * .4f;
            var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

            var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _platformLayer);

            Debug.DrawRay(hitPoint.point, hitPoint.normal);

            _isOnPlatform = hitPoint;
        }

        private void SlopeCheck()
        {
            Vector2 checkPos = transform.position - new Vector3(0f, _playerCollider.size.y / 2);

            SlopeCheckHorizontal(checkPos);
            SlopeCheckVertical(checkPos);
        }

        private void SlopeCheckHorizontal(Vector2 checkPos)
        {
            RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, _groundLayer);
            RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, _groundLayer);

            if (slopeHitFront)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
            }
            else if (slopeHitBack)
            {
                _isOnSlope = true;
                _slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
            }
            else
            {
                _slopeSideAngle = 0f;
                _isOnSlope = false;
            }
        }

        private void SlopeCheckVertical(Vector2 checkPos)
        {
            RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, _groundLayer);

            if (hit)
            {
                _slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;
                _slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (_slopeDownAngle != _slopeDownAngleOld)
                    _isOnSlope = true;

                _slopeDownAngleOld = _slopeDownAngle;

                Debug.DrawRay(hit.point, _slopeNormalPerp, Color.red);
                Debug.DrawRay(hit.point, hit.normal, Color.white);
            }
        }
    }
}