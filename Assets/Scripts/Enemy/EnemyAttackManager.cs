using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static Action<LayerMask> shootCall;

    [SerializeField] private GameObject _enemyHand;
    private Player.Pivot _pivot;

    [SerializeField] Item.Weapon.RangeWeaponInterface rangeWeapon;

    private CapsuleCollider2D _collider;
    private AlarmManager _alarmManager;

    private LayerMask _hitLayer;
    private LayerMask _playerLayers;

    public bool _canAttack = true;

    private void Start()
    {
        _pivot = _enemyHand.GetComponent<Player.Pivot>();
        _collider = gameObject.GetComponent<CapsuleCollider2D>();

        _hitLayer = LayerMask.GetMask("Player", "PlayerThroughPlatform", "Ground");
        _playerLayers = LayerMask.GetMask("Player", "PlayerThroughPlatform");

        shootCall += rangeWeapon.Shoot;

        _alarmManager = GetComponent<AlarmManager>();

        rangeWeapon.GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(rangeWeapon.GetComponent<Rigidbody2D>());
    }

    public void FixedUpdate()
    {
        if (!_canAttack)
            return;

        float forwardAngle = (Mathf.PI / 2 * transform.localScale.x * Mathf.Rad2Deg) + 45;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        for (int ray = 90; ray > 0; ray--)
        {
            float angleDir = (forwardAngle + ray) * Mathf.Deg2Rad;
            var directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));

            //Debug.DrawRay(_collider.bounds.center, directionToShoot, Color.blue);
            RaycastHit2D hitInfo = Physics2D.Raycast(_collider.bounds.center, directionToShoot, 100f, _hitLayer);

            if (!hitInfo)
                continue;

            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                continue;

            Player.PlayerController player = hitInfo.transform.GetComponent<Player.PlayerController>();

            if (player != null)
            {
                hits.Add(hitInfo);                

                if (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Calm || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Concerned || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Aware)
                {
                    _alarmManager.PlayerHasBeenSpoted(player.transform);
                    return;
                }
            }
        }

        if (hits.Count != 0 && (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Alarmed || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Aware))
        {
            _pivot.ChangeRotation(hits[hits.Count/2].point);
            shootCall?.Invoke(_playerLayers);
        }
    }

    public GameObject GetHand()
    {
        return _enemyHand;
    }
}
