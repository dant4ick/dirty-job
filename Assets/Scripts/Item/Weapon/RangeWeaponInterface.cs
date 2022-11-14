using System.Collections;
using UnityEngine;

namespace Item.Weapon
{
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
                Vector2 firePointPosition = _firePoint.position;
                Vector2 directionToShoot = pointToShoot - firePointPosition;
                float turn = Random.Range(-rangeWeapon.SpreadDegrees, rangeWeapon.SpreadDegrees) * Mathf.Deg2Rad;

                // Applying a spread to the bullet using polar coordinate system 
                float angleDir = Mathf.Atan2(directionToShoot.y, directionToShoot.x) + turn;
                directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));
                
                // TODO: fix shooting towards shooter

                RaycastHit2D hitInfo = Physics2D.Raycast(firePointPosition, directionToShoot);

                Debug.DrawRay(firePointPosition, directionToShoot, Color.black, 10f);

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
}