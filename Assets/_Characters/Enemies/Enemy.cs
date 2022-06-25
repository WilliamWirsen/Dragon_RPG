﻿using RPG.Core;
using RPG.Weapons;
using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 7f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] TextMesh _damageText;
        [SerializeField] GameObject _popupTextGameObject;

        float currentHealthPoints;
        AICharacterControl aiCharacterControl = null;
        GameObject player = null;
        bool isAttacking = false;

        public float healthAsPercentage
        {
            get
            {
                float healthAsPercentage = currentHealthPoints / maxHealthPoints;
                return healthAsPercentage;
            }
        }
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, aiCharacterControl.transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating("FireProjectile", 0f, secondsBetweenShots);
            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }

            if (distanceToPlayer <= chaseRadius)
            {
                aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                aiCharacterControl.SetTarget(aiCharacterControl.transform);
            }
        }

        public static ScreenPopupText CreateDamagePopup(Vector3 position, int damage, GameObject damagePopupText)
        {
            var damagePopupTextTransform = Instantiate(damagePopupText, position, Quaternion.identity);
            ScreenPopupText popupText = damagePopupTextTransform.GetComponent<ScreenPopupText>();
            popupText.Setup(damage);

            return popupText;
        }

        public void TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if(damage >= 0)
            {
                CreateDamagePopup(gameObject.transform.position, (int)Math.Ceiling(damage), _popupTextGameObject);
            }
            else
            {
                // Not yet implemented
            }
            if (currentHealthPoints <= 0) 
            { 
                Destroy(gameObject); 
            }
        }

        private void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            // Draw move sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);

        }
    }
}

