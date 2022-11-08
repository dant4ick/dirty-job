using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    protected int AttackDamage { get; set; }
    protected LayerMask EnemyLayers { get; }

    public delegate void Effects(Enemy enemy);
    protected Effects effects;

    public virtual void Attack() { }
}