using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Item
{
    public class RangeWeaponInterface : ItemInterface
    {
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
            if (_rangeWeapon == null)
            {
                return;
            }
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

            SoundManager.PlayWeaponSound(_rangeWeapon.SoundOnAttack);

            AlarmManager.AlarmEnemiesByLoudSound(transform, 200);

            List<RaycastHit2D> hitedEnemies = new List<RaycastHit2D>();

            for (int bullet = 0; bullet < _rangeWeapon.NumberOfBulletsPerShot; bullet++)
            {
                Vector2 firePointPosition = _firePoint.position;
                Vector2 directionToShoot = _firePoint.transform.rotation * Vector2.right * transform.lossyScale.x;

                float turn = Random.Range(-_rangeWeapon.SpreadDegrees, _rangeWeapon.SpreadDegrees) * Mathf.Deg2Rad;

                // Applying a spread to the bullet using polar coordinate system 
                float angleDir = Mathf.Atan2(directionToShoot.y, directionToShoot.x) + turn;
                directionToShoot = new Vector2(Mathf.Cos(angleDir), Mathf.Sin(angleDir));

                if (_rangeWeapon.Penetration > 1)
                {
                    for (int penetration = 0; penetration < _rangeWeapon.Penetration; penetration++)
                    {
                        RaycastHit2D hitInfo;

                        hitInfo = Physics2D.Raycast(firePointPosition, directionToShoot, _rangeWeapon.Distance, _rangeWeapon.EnemyLayers);

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
                }
                else
                {
                    RaycastHit2D hitInfo;

                    hitInfo = Physics2D.Raycast(firePointPosition, directionToShoot, _rangeWeapon.Distance, enemyLayer);

                    Debug.DrawRay(firePointPosition, directionToShoot, Color.black, 100f);

                    if (hitInfo)
                    {
                        if (hitedEnemies.Count > 0)
                        {
                            if (!DublicationCheck(hitedEnemies, hitInfo))
                            {
                                hitedEnemies.Add(hitInfo);
                            }
                        }
                        else
                        {
                            hitedEnemies.Add(hitInfo);
                        }
                    }
                }
            }

            if (_rangeWeapon.Penetration < 2)
            {
                foreach (RaycastHit2D hit in hitedEnemies)
                {
                    HealthManager mortal = hit.transform.GetComponent<HealthManager>();

                    if (mortal != null)
                    {
                        Vector2 hitNormal = hit.normal * -1;

                        Quaternion rotation = Quaternion.Euler(hitNormal.x, hitNormal.y, 0f);

                        mortal.TakeDamage();
                        mortal.TakeBleed(_rangeWeapon.ParticleSystem, hit.point, rotation);
                    }
                }
            }

            _lastTimeAttack = Time.time;
        }

        private bool DublicationCheck (List<RaycastHit2D> hitedEnemies, RaycastHit2D hitInfo)
        {
            foreach(RaycastHit2D hit in hitedEnemies)
            {
                if (hit.rigidbody == hitInfo.rigidbody)
                {
                    return true;
                }
            }
            return false;
        }

        private IEnumerator Reload()
        {
            _isReloading = true;
            SoundManager.PlayWeaponSound(_rangeWeapon.SoundOnReload);

            yield return new WaitForSeconds(_rangeWeapon.ReloadTime);
            _currentAmmo = _rangeWeapon.MaxAmmo;

            _isReloading = false;
        }
    }
}