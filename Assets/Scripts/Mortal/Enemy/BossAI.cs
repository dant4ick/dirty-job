using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BossAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public float pathUpdateSeconds = 0.5f;

    [SerializeField] private List<Transform> patrolPath;
    private int _currentPatrolPoint;

    private Path path;
    private int currentWaypoint = 0;

    private Seeker _seeker;

    [Header("Physics")]
    public float speed = 8;
    public float nextWaypointDistance = 0.5f;

    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    [SerializeField] private float slopeCheckDistance;
    private float _slopeDownAngle;
    private float _slopeDownAngleOld;
    private float _slopeSideAngle;
    private Vector2 _slopeNormalPerp;
    private bool _isOnSlope;

    private bool _isOnGround;
    private bool _isOnPlatform;
    private static int _groundLayer;
    private static int _platformLayer;

    private float _horizontalMovement = 0;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool directionLookEnabled = true;
    private AlarmManager _alarmManager;
    private EnemyAttackManager _attackManager;

    [Header("Friction materials")]
    [SerializeField] private PhysicsMaterial2D zeroFriction;
    [SerializeField] private PhysicsMaterial2D maxFriction;

    [SerializeField] private Animator animator;
    private static readonly int EnemyIdle = Animator.StringToHash("Enemy_Idle");
    private static readonly int EnemyRun = Animator.StringToHash("Enemy_Run");
    private static readonly int EnemyIdleNoHand = Animator.StringToHash("Enemy_IdleNoHand");
    private int _currentAnimation;
    public void Start()
    {
        _seeker = GetComponent<Seeker>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();

        _groundLayer = LayerMask.GetMask("Ground");
        _platformLayer = LayerMask.GetMask("Platform");

        _alarmManager = GetComponent<AlarmManager>();
        _attackManager = GetComponent<EnemyAttackManager>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        PlatformCheck();
        SlopeCheck();

        bool attackCheck = _attackManager.CheckForAttack();

        if (attackCheck)
        {
            ChangeAnimationState(EnemyIdleNoHand);
            _rigidbody.sharedMaterial = maxFriction;
            return;
        }
        else if (!followEnabled)
        {
            ChangeAnimationState(EnemyIdle);
            _rigidbody.sharedMaterial = maxFriction;
            return;
        }
        if (_isOnGround || _isOnPlatform)
        {
            GetComponent<EnemyAttackManager>().GetHand().SetActive(false);
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (!_seeker.IsDone())
            return;

        if (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Calm)
        {
            if (patrolPath.Count > 1)
                CalmPatrolling();
            else if (_alarmManager.target != null)
                GoToSoundCalm();
        }
        else if (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Aware || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Concerned)
        {
            _seeker.StartPath(transform.position, _alarmManager.soundPosition, OnPathComplete);
        }
    }

    private void GoToSoundCalm()
    {
        GraphNode startNode = AstarPath.active.GetNearest(transform.position).node;
        _seeker.StartPath((Vector3)startNode.position, _alarmManager.target.position, OnPathComplete);
    }

    private void CalmPatrolling()
    {
        GraphNode startNode = AstarPath.active.GetNearest(transform.position).node;
        _seeker.StartPath((Vector3)startNode.position, patrolPath[_currentPatrolPoint].transform.position, OnCalmPathComplete);
    }

    private void PathFollow()
    {
        if (path == null)
        {
            ChangeAnimationState(EnemyIdle);
            _rigidbody.sharedMaterial = maxFriction;
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            ChangeAnimationState(EnemyIdle);
            _rigidbody.sharedMaterial = maxFriction;
            return;
        }

        if (_isOnPlatform)
        {
            if (path.vectorPath[currentWaypoint].y < path.vectorPath[currentWaypoint - 1].y)
            {
                gameObject.layer = LayerMask.NameToLayer("EnemyThroughPlatform");
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }

        _rigidbody.sharedMaterial = zeroFriction;

        if (path.vectorPath[currentWaypoint].x > transform.position.x)
        {
            ChangeAnimationState(EnemyRun);

            _horizontalMovement = 1;

            if (_isOnSlope)
            {
                _rigidbody.velocity = new Vector2(-_horizontalMovement * speed * _slopeNormalPerp.x, -_horizontalMovement * speed * _slopeNormalPerp.y);
            }
            else
            {
                _rigidbody.velocity = new Vector2(_horizontalMovement * speed, 0f);
            }
        }
        else if (path.vectorPath[currentWaypoint].x < transform.position.x)
        {
            ChangeAnimationState(EnemyRun);

            _horizontalMovement = -1;

            if (_isOnSlope)
            {
                _rigidbody.velocity = new Vector2(-_horizontalMovement * speed * _slopeNormalPerp.x, -_horizontalMovement * speed * _slopeNormalPerp.y);

            }
            else
            {
                _rigidbody.velocity = new Vector2(_horizontalMovement * speed, 0f);
            }
        }

        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        if (directionLookEnabled)
        {
            if (_rigidbody.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (_rigidbody.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void OnCalmPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;

            if (path.path.Count > 1)
                currentWaypoint = 1;

            if (path.vectorPath.Count <= 2)
                _currentPatrolPoint++;
            if (_currentPatrolPoint >= patrolPath.Count)
                _currentPatrolPoint = 0;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;

            if (path.path.Count > 1)
                currentWaypoint = 1;
        }
    }

    private void GroundCheck()
    {
        var bounds = _collider.bounds;
        var radius = bounds.size.x * .4f;
        var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

        var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _groundLayer);

        Debug.DrawRay(hitPoint.point, hitPoint.normal);

        _isOnGround = hitPoint;
    }

    private void PlatformCheck()
    {
        var bounds = _collider.bounds;
        var radius = bounds.size.x * .4f;
        var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

        var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _platformLayer);

        Debug.DrawRay(hitPoint.point, hitPoint.normal);

        _isOnPlatform = hitPoint;
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f, _collider.size.y / 2);

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
    private void ChangeAnimationState(int newAnimation)
    {
        if (newAnimation == _currentAnimation) return;
        animator.Play(newAnimation);
        _currentAnimation = newAnimation;
    }
}