using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RangeWeapon", menuName = "ScriptableObjects/Weapons/RangeWeapon")]
public class RangeWeapon : Weapon
{
    #region BASIC RANGE WEAPON STATS
        [Header ("Basic Range Weapon Stats")] 
        [SerializeField] private float spreadDegrees;
        [SerializeField] private float distance;
        [SerializeField] private float penetration;
        [SerializeField] private int numberOfBulletsPerShot;
        [SerializeField] private int maxAmmo;
        [SerializeField] private float reloadTime;
    #endregion

    public float SpreadDegrees { get { return spreadDegrees; } }
    public float Distance { get { return distance; } }
    public float Penetration { get { return penetration; } }
    public int NumberOfBulletsPerShot { get { return numberOfBulletsPerShot; } }

    public int MaxAmmo { get { return maxAmmo; } }
    public float ReloadTime { get { return reloadTime; } }

    // Start is called before the first frame update

    //protected void BulletHit(Enemy enemy)
    //{
    //    enemy.TakeDamage(AttackDamage);
    //}
}
