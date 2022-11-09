using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MeleeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        AttackDamage = 100;
        AttackRange = 0.75f;

        FireRate = 0.5f;

        effects += Bleed;

        base.Start();
    }

    private void Bleed(Enemy enemy)
    {
        enemy.TakeBleed();
    }
}
