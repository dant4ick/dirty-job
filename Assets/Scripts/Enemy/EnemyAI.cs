using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private List<Transform> patrolPath;
    private int _currentPatrolPoint;
    public Transform target;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 0.01f;

    [Header("Custom Behavior")]
    public bool calm;
    public bool followEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    private float _horizontalMovement = 0;
    private static int _groundLayer;


    [Header("Friction materials")]
    [SerializeField] private PhysicsMaterial2D zeroFriction;
    [SerializeField] private PhysicsMaterial2D maxFriction;

    public void Start()
    {        
        seeker = GetComponent<Seeker>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _groundLayer = LayerMask.GetMask("Ground");
        calm = true;

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (followEnabled && IsGrounded())
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (!seeker.IsDone())
            return;


        if (calm)
        {
            CalmPatrolling();
        }
        else
            seeker.StartPath(transform.position, target.position, OnPathComplete);

        //currentWaypoint++;
    }

    private void CalmPatrolling()
    {
        GraphNode startNode = AstarPath.active.GetNearest(transform.position).node;
        seeker.StartPath((Vector3)startNode.position, patrolPath[_currentPatrolPoint].transform.position, OnCalmPathComplete);
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        if (path.vectorPath[currentWaypoint].x > transform.position.x)
        {
            _horizontalMovement = 1;

            _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
        }
        else if (path.vectorPath[currentWaypoint].x < transform.position.x)
        {
            _horizontalMovement = -1;

            _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
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
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (_rigidbody.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
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

    private bool IsGrounded()
    {
        var bounds = _collider.bounds;
        var radius = bounds.size.x * .4f;
        var origin = (Vector2)bounds.center - new Vector2(0f, (bounds.size.y / 2) - radius);

        var hitPoint = Physics2D.CircleCast(origin, radius, Vector2.down, .2f, _groundLayer);

        Debug.DrawRay(hitPoint.point, hitPoint.normal);

        return hitPoint;
    }
}