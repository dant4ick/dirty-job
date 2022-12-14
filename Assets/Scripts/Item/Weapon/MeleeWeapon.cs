using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MeleeWeapon", menuName = "ScriptableObjects/Weapons/MeleeWeapon")]
public class MeleeWeapon : Weapon
{
    [Header("Basic Melee Weapon Stats")]
    [SerializeField] private float _attackRange;
    [SerializeField] public List<string> effects;

    public float AttackRange { get { return _attackRange; } }
}
