using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : RangeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AttackDamage = 100;

        Spread = 0.0825f;
        Penetration = 0f;
        NumberOfBullets = 1;
    }
}
