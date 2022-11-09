using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : RangeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        AttackDamage = 100;
        Spread = 0.0825f;
        Penetration = 0f;
        FireRate = 0.3f;
        NumberOfBulletsPerShot = 1;

        MaxAmmo = 8;
        ReloadTime = 2f;

        base.Start();
    }
}
