using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MeleeWeapon : Weapon
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    protected float AttackRange { get; set; }

    protected virtual void Start()
    {
        effects += Damage;
    }

    private void Damage(Collider2D enemy)
    {
        enemy.GetComponent<Enemy>().TakeDamage(AttackDamage);
    }

    public override void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            effects(enemy);
        }
    }
}
