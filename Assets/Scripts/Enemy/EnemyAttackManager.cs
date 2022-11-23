using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static Action shootCall;

    [SerializeField] private GameObject enemyHand;
    private Player.Pivot _pivot;

    [SerializeField] Item.Weapon.RangeWeaponInterface rangeWeapon;

    private CapsuleCollider2D _collider;
    private LayerMask _playerLayer;
    

    private void Start()
    {
        _pivot = enemyHand.GetComponent<Player.Pivot>();
        _collider = gameObject.GetComponent<CapsuleCollider2D>();
        _playerLayer = LayerMask.GetMask("Player");

        shootCall += rangeWeapon.Shoot;

        rangeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(rangeWeapon.GetComponent<Rigidbody2D>());
    }

    public void FixedUpdate()
    {
        float forwardAngle = (Mathf.PI / 2 * transform.localScale.x * Mathf.Rad2Deg) + 45;
        for (int ray = 90; ray > 0; ray--)
        {
            float angleDir = (forwardAngle + ray) * Mathf.Deg2Rad;
            var directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));

            //Debug.DrawRay(_collider.bounds.center, directionToShoot, Color.blue);
            RaycastHit2D hitInfo = Physics2D.Raycast(_collider.bounds.center, directionToShoot, 100f, _playerLayer);

            if (hitInfo)
            {
                Player.PlayerController player = hitInfo.transform.GetComponent<Player.PlayerController>();

                if (player != null)
                {
                    _pivot.ChangeRotation(hitInfo.point);
                    shootCall?.Invoke();
                    return;
                }
            }
        }
    }
}
