using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : Item.Item
{
    [SerializeField] private LayerMask _enemyLayers;

    #region BASIC WEAPON STATS
        [Header("Basic Weapon Stats")]
        [SerializeField] private float _attackRate;
    #endregion

    [Header("Particle system")]
    [SerializeField] private GameObject _particleSystem;

    public GameObject ParticleSystem { get { return _particleSystem; } }
    public float AttackRate { get { return _attackRate; } }

    public LayerMask EnemyLayers { get { return _enemyLayers; } }
}