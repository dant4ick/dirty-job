using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MeleeWeapon : Weapon
{
    [SerializeField] private Transform attackPoint;
    protected float AttackRange { get; set; }

    protected virtual void Start()
    {
        effects += Damage;
    }

    private void Damage(Enemy enemy)
    {
        enemy.TakeDamage(AttackDamage);
    }

    public override void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, EnemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            effects(enemy.GetComponent<Enemy>());
        }
    }
}
