using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item.Item
{
    [SerializeField] private LayerMask _enemyLayers;

    #region BASIC WEAPON STATS
        [Header("Basic Weapon Stats")]
        [SerializeField] private int _attackDamage;
        [SerializeField] private float _attackRate;
    #endregion

    public int AttackDamage { get { return _attackDamage; } }
    public float AttackRate { get { return _attackRate; } }

    protected LayerMask EnemyLayers
    {
        get { return _enemyLayers; }
    }

    public delegate void Effects(Enemy enemy);
    public Effects effects;
}