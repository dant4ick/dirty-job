using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    public Transform target;
    public float activateDistance = 50f;
    public float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    public float speed = 200f;
    public float nextWaypointDistance = 5f;

    [Header("Custom Behavior")]
    public bool followEnabled = true;
    public bool directionLookEnabled = true;

    private Path path;
    private int currentWaypoint = 0;
    private Seeker seeker;
    private Rigidbody2D _rigidbody;
    private CapsuleCollider2D _collider;

    private float _horizontalMovement = 0;
    private bool _pathUpdated = false;
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

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled && IsGrounded())
        {
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(_rigidbody.position, target.position, OnPathComplete);
            _pathUpdated = true;
        }
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

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - _rigidbody.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        //_rigidbody.AddForce(force);


        float distance = Vector2.Distance(_rigidbody.position, path.vectorPath[currentWaypoint]);

        if (_pathUpdated == true)
        {
            _pathUpdated = false;
            _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
        }
        else
        {
            if (path.vectorPath[currentWaypoint + 1] != null)
            {
                if (path.vectorPath[currentWaypoint+1].x > _rigidbody.position.x)
                {
                    _horizontalMovement = 1;

                    _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
                }
                else if (path.vectorPath[currentWaypoint+1].x < _rigidbody.position.x)
                {
                    _horizontalMovement = -1;

                    _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
                }
            }
            else
            {
                if (path.vectorPath[currentWaypoint].x > _rigidbody.position.x)
                {
                    _horizontalMovement = 1;

                    _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
                }
                else if (path.vectorPath[currentWaypoint].x < _rigidbody.position.x)
                {
                    _horizontalMovement = -1;

                    _rigidbody.velocity = new Vector2(_horizontalMovement * speed, _rigidbody.velocity.y);
                }
            }


        }

        //float distance = Vector2.Distance(_rigidbody.position, path.vectorPath[currentWaypoint]);
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

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
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

