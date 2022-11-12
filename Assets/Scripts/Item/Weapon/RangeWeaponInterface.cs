using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponInterface : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Transform _firePoint;
    [SerializeField] public RangeWeapon rangeWeapon;

    private float _lastTimeAttack;

    private void Start()
    {
        _spriteRenderer.sprite = rangeWeapon.Sprite;
        PlayerShoot.shootInput += Shoot;
    }

    public void Shoot(Vector2 pointToShoot)
    {
        if (rangeWeapon.IsReloading)
        {
            return;
        }

        if (Time.time < _lastTimeAttack + rangeWeapon.AttackRate)
        {
            return;
        }

        if (rangeWeapon.CurrentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        rangeWeapon.CurrentAmmo--;

        for (int bullet = 0; bullet < rangeWeapon.NumberOfBulletsPerShot; bullet++)
        {
            Vector2 offset = new Vector2(Random.Range(0f, rangeWeapon.Spread), Random.Range(-rangeWeapon.Spread, rangeWeapon.Spread));
            RaycastHit2D hitInfo = Physics2D.Raycast(_firePoint.position, pointToShoot + offset);

            Debug.DrawRay(_firePoint.position, pointToShoot + offset, Color.black, 10f);

            if (hitInfo)
            {
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(rangeWeapon.AttackDamage);
                }
            }

            _lastTimeAttack = Time.time;
        }
    }

    IEnumerator Reload()
    {
        rangeWeapon.IsReloading = true;

        yield return new WaitForSeconds(rangeWeapon.ReloadTime);
        rangeWeapon.CurrentAmmo = rangeWeapon.MaxAmmo;

        rangeWeapon.IsReloading = false;
    }
}