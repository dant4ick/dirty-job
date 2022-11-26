using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item.Weapon
{
    public class RangeWeaponInterface : ItemInterface
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _firePoint;
        private RangeWeapon _rangeWeapon;
        private int _currentAmmo;
        private bool _isReloading = false;
        private float _lastTimeAttack;

        protected override void Start()
        {
            base.Start();

            _rangeWeapon = (RangeWeapon)Item;
            _currentAmmo = _rangeWeapon.MaxAmmo;
        }

        public void Shoot(LayerMask enemyLayer)
        {
            if (_isReloading)
            {
                return;
            }

            if (Time.time < _lastTimeAttack + _rangeWeapon.AttackRate)
            {
                return;
            }

            if (_currentAmmo == 0)
            {
                StartCoroutine(Reload());
                return;
            }

            _currentAmmo--;

            AlarmManager.AlarmEnemiesByLoudSound(transform, 200);

            for (int bullet = 0; bullet < _rangeWeapon.NumberOfBulletsPerShot; bullet++)
            {
                Vector2 firePointPosition = _firePoint.position;
                Vector2 directionToShoot = _firePoint.transform.rotation * Vector2.right;
                float turn = Random.Range(-_rangeWeapon.SpreadDegrees, _rangeWeapon.SpreadDegrees) * Mathf.Deg2Rad;

                // Applying a spread to the bullet using polar coordinate system 
                float angleDir = Mathf.Atan2(directionToShoot.y, directionToShoot.x) + turn;
                directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));

                RaycastHit2D hitInfo = Physics2D.Raycast(firePointPosition, directionToShoot, enemyLayer);

                Debug.DrawRay(firePointPosition, directionToShoot, Color.black, 100f);
                
                if (hitInfo)
                {
                    HealthManager mortal = hitInfo.transform.GetComponent<HealthManager>();

                    if (mortal != null)
                    {
                        Vector2 hitNormal = hitInfo.normal * -1;

                        Quaternion rotation = Quaternion.Euler(hitNormal.x, hitNormal.y, 0f);

                        mortal.TakeDamage();
                        mortal.TakeBleed(_rangeWeapon.ParticleSystem, hitInfo.point, rotation);
                    }
                }
            }

            _lastTimeAttack = Time.time;
        }

        private IEnumerator Reload()
        {
            _isReloading = true;

            yield return new WaitForSeconds(_rangeWeapon.ReloadTime);
            _currentAmmo = _rangeWeapon.MaxAmmo;

            _isReloading = false;
        }
    }
}