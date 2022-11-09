using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    protected int AttackDamage { get; set; }
    protected float FireRate { get; set; }
    protected float LastTimeAttack { get; set; }
    protected LayerMask EnemyLayers
    {
        get { return enemyLayers; }
    }

    public delegate void Effects(Enemy enemy);
    protected Effects effects;

    public virtual void Attack() { }
}