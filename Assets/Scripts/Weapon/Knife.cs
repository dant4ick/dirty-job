using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MeleeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AttackDamage = 100;
        AttackRange = 0.75f;

        effects += Bleed;
    }

    private void Bleed(Collider2D enemy)
    {
        enemy.GetComponent<Enemy>().TakeBleed();
    }
}
