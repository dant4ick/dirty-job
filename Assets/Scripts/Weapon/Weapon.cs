using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Weapon : MonoBehaviour
{
    protected int AttackDamage { get; set; }

    public delegate void Effects(Collider2D enemy);
    protected Effects effects;

    public virtual void Attack() { }
}