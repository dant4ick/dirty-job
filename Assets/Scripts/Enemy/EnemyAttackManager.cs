using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackManager : MonoBehaviour
{
    public static Action<LayerMask> shootCall;

    [SerializeField] private GameObject _enemyHand;
    private Player.Pivot _pivot;

    public Item.RangeWeaponInterface rangeWeapon;

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
    private void OnDestroy()
    {
        shootCall -= rangeWeapon.Shoot;
    }

    public bool CheckForAttack()
    {
        if (!_canAttack)
            return false;

        float forwardAngle = (Mathf.PI / 2 * transform.localScale.x * Mathf.Rad2Deg * -1) + 45;

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        for (int ray = 90; ray > 0; ray--)
        {
            float angleDir = (forwardAngle + ray) * Mathf.Deg2Rad;
            var directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));

            //Debug.DrawRay(_collider.bounds.center + new Vector3(0f, 0.4375f, 0f), directionToShoot, Color.blue);
            RaycastHit2D hitInfo = Physics2D.Raycast(_collider.bounds.center + new Vector3(0f, 0.4375f, 0f), directionToShoot, 15f, _hitLayer);

            if (!hitInfo)
                continue;

            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                continue;

            Player.PlayerController player = hitInfo.transform.GetComponent<Player.PlayerController>();

            if (player != null)
            {
                //Debug.DrawRay(_collider.bounds.center + new Vector3(0f, 0.4375f, 0f), directionToShoot, Color.blue, 100f);
                hits.Add(hitInfo);

                if (SpotPlayer(player))
                    return false;
            }
        }

        if (Attack(hits))
            return true;

        return false;
    }

    private bool SpotPlayer(Player.PlayerController playerToSpot)
    {
        if (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Calm || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Concerned || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Aware)
        {
            _alarmManager.PlayerHasBeenSpoted(playerToSpot.transform);
            return true;
        }
        return false;
    }

    private bool Attack(List<RaycastHit2D> hitsWithPlayer)
    {
        if (hitsWithPlayer.Count != 0 && (_alarmManager.alarmLevel == AlarmManager.AlarmLevel.Alarmed || _alarmManager.alarmLevel == AlarmManager.AlarmLevel.Aware))
        {
            _enemyHand.gameObject.SetActive(true);
            _pivot.ChangeRotation(hitsWithPlayer[hitsWithPlayer.Count / 2].point);
            shootCall?.Invoke(_hitLayer);

            return true;
        }

        return false;
    }

    public GameObject GetHand()
    {
        return _enemyHand;
    }
}
