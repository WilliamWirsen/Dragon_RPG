﻿using RPG.Core;
using UnityEngine;

namespace RPG.Weapons
{
    public class Projectile : MonoBehaviour
    {

        [SerializeField] float projectileSpeed;
        [SerializeField] GameObject shooter;

        const float DESTROY_DELAY = 0.01f;
        float damageCaused;

        public void SetShooter(GameObject shooter)
        {
            this.shooter = shooter;
        }

        public void SetDamage(float damage)
        {
            damageCaused = damage;
        }

        public float GetDefaultLaunchSpeed()
        {
            return projectileSpeed;
        }

        void OnCollisionEnter(Collision collision)
        {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer && layerCollidedWith != 10)
            {
                DamageDamageable(collision);
            }
        }

        private void DamageDamageable(Collision collision)
        {
            Component damageableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageableComponent)
            {
                (damageableComponent as IDamageable).TakeDamage(damageCaused);
            }
            Destroy(gameObject, DESTROY_DELAY);
        }
    }
}


