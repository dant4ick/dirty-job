using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeWeapon", menuName = "ScriptableObjects/Weapons/RangeWeapon")]
public class RangeWeapon : Weapon
{
    #region BASIC RANGE WEAPON STATS
        [Header ("Basic Range Weapon Stats")] 
        [SerializeField] private float spreadDegrees;
        [SerializeField] private float _penetration;
        [SerializeField] private int _numberOfBulletsPerShot;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private int _currentAmmo;
        [SerializeField] private float _reloadTime;
        [SerializeField] private bool _isReloading;
    #endregion

    public float SpreadDegrees { get { return spreadDegrees; } }
    public float Penetration { get { return _penetration; } }
    public int NumberOfBulletsPerShot { get { return _numberOfBulletsPerShot; } }

    public int MaxAmmo { get { return _maxAmmo; } }
    public int CurrentAmmo { get { return _currentAmmo; } set { _currentAmmo = value; } }
    public float ReloadTime { get { return _reloadTime; } }
    public bool IsReloading { get { return _isReloading; } set { _isReloading = value; } }

    // Start is called before the first frame update

    //protected void BulletHit(Enemy enemy)
    //{
    //    enemy.TakeDamage(AttackDamage);
    //}
}
