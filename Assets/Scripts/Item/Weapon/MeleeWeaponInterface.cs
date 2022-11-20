using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Weapon
{
    public class MeleeWeaponInterface : ItemInterface
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        public Transform attackPoint;
        private MeleeWeapon _meleeWeapon;
        private float _lastTimeAttack;

        private delegate void Effects(Enemy enemy);
        private Effects effects;

        private void Start()
        {
            _meleeWeapon = (MeleeWeapon)Item;

            effects += DealDamage;
            
            foreach (String effect in _meleeWeapon.effects)
            {
                if (effect == "Stun")
                    effects += DealStun;
                if (effect == "Bleed")
                    return;
            }     
        }

        private void DealDamage(Enemy enemy)
        {
            enemy.TakeDamage(_meleeWeapon.AttackDamage);
        }

        private void DealStun(Enemy enemy)
        {
            enemy.TakeStun(5);
        }

        public void Attack()
        {
            if (Time.time < _lastTimeAttack + _meleeWeapon.AttackRate)
            {
                return;
            }

            Debug.Log(_meleeWeapon.EnemyLayers.value);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, _meleeWeapon.AttackRange, _meleeWeapon.EnemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                effects(enemy.GetComponent<Enemy>());
            }

            _lastTimeAttack = Time.time;
        }
    }
}
