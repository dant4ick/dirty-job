using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MeleeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        AttackDamage = 100;
        AttackRange = 0.75f;

        FireRate = 1f;

        effects += TakeStun;

        base.Start();
    }

    private void TakeStun(Enemy enemy)
    {
        enemy.TakeStun(1f);
    }
}
