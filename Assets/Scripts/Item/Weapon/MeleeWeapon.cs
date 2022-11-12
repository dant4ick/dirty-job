using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "ScriptableObjects/Weapons/MeleeWeapon")]
abstract public class MeleeWeapon : Weapon
{
    protected float AttackRange { get; set; }

    //private void Damage(Enemy enemy)
    //{
    //    enemy.TakeDamage(AttackDamage);
    //}

    //public override void Attack()
    //{
    //    if (Time.time < LastTimeAttack + FireRate)
    //    {
    //        return;
    //    }

    //    Debug.Log(EnemyLayers.value);

    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, AttackRange, EnemyLayers);

    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        effects(enemy.GetComponent<Enemy>());
    //    }

    //    LastTimeAttack = Time.time;
    //}
}
