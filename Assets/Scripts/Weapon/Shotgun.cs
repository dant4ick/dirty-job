using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : RangeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AttackDamage = 50;

        Spread = 0.7f;
        Penetration = 1f;
        NumberOfBullets = 8;
    }
}
