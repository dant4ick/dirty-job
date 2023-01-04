using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public class MeleeWeaponInterface : ItemInterface
    {
        public Transform attackPoint;
        private MeleeWeapon _meleeWeapon;
        private float _lastTimeAttack;

        private delegate void Effects(HealthManager enemy);
        private Effects effects;

        protected override void Start()
        {
            base.Start();

            _meleeWeapon = (MeleeWeapon)Item;

            effects += DealDamage;  
        }

        private void DealDamage(HealthManager enemy)
        {
            enemy.TakeDamage();
            enemy.TakeBleed(_meleeWeapon.ParticleSystem);
        }

        public void Attack()
        {
            if (Time.time < _lastTimeAttack + _meleeWeapon.AttackRate)
            {
                return;
            }

            SoundManager.PlayWeaponSound(_meleeWeapon.SoundOnAttack);

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, _meleeWeapon.AttackRange, _meleeWeapon.EnemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                effects(enemy.GetComponent<HealthManager>());
            }

            Collider2D[] hitProps = Physics2D.OverlapCircleAll(attackPoint.position, _meleeWeapon.AttackRange, LayerMask.GetMask("Breakable"));
            foreach (Collider2D prop in hitProps)
            {
                prop.GetComponent<BreakObject>().Break();
            }

            _lastTimeAttack = Time.time;
        }
    }
}
