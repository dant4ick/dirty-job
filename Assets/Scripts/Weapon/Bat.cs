using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MeleeWeapon
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        AttackDamage = 100;
        AttackRange = 0.75f;

        effects += Stun;
    }

    private void Stun(Collider2D enemy)
    {
        enemy.GetComponent<Enemy>().TakeStun(1f);
    }
}
