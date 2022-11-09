using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : RangeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        AttackDamage = 50;
        Spread = 0.7f;
        Penetration = 1f;
        FireRate = 0.5f;
        NumberOfBulletsPerShot = 8;

        MaxAmmo = 2;
        ReloadTime = 5f;

        base.Start();
    }
}
