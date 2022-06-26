using RPG.Weapons;
using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters.NPCs.Enemies
{
    [RequireComponent(typeof(NonControllableCharacter))]
    [RequireComponent(typeof(Animator))]
    public class Enemy : MonoBehaviour
    {

        [SerializeField]
        float _ChaseRadius = 7f;

        [SerializeField]
        float _AttackRadius = 4f;

        [SerializeField]
        float _DamagePerShot = 9f;

        [SerializeField]
        float _SecondsBetweenShots = 0.5f;

        [SerializeField]
        [Tooltip("Only used for ranged enemies")]
        GameObject _ProjectileToUse;

        [SerializeField]
        [Tooltip("Only used for ranged enemies")]
        GameObject _ProjectileSocket;

        [SerializeField]
        Vector3 _AimOffset = new(0, 1f, 0);

        [SerializeField]
        TextMesh _DamageText;
        [SerializeField]
        EnemyType _EnemyType;

        private AICharacterControl _AiCharacterControl = null;
        private NonControllableCharacter _NonControllableCharacter = null;
        private Animator _Animator = null;
        private GameObject _Player = null;
        private static bool _IsAttacking = false;

        private void Start()
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            _AiCharacterControl = GetComponent<AICharacterControl>();
            _NonControllableCharacter = GetComponent<NonControllableCharacter>();
            _Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_NonControllableCharacter.IsAlive() == false)
                return;

            float distanceToPlayer = Vector3.Distance(_Player.transform.position, _AiCharacterControl.transform.position);

            if (distanceToPlayer <= _AttackRadius && !_IsAttacking)
            {
                switch (_EnemyType)
                {
                    case EnemyType.Ranged:
                        _IsAttacking = true;
                        InvokeRepeating(nameof(FireProjectile), 0f, _SecondsBetweenShots);
                        if (distanceToPlayer > _AttackRadius)
                        {
                            _IsAttacking = false;
                            CancelInvoke();
                        }

                        break;
                    case EnemyType.Melee:
                        if (distanceToPlayer < _AttackRadius)
                        {
                            _IsAttacking = true;
                            _NonControllableCharacter.AttackTarget(_Player, _DamagePerShot);
                        }
                        _IsAttacking = false;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            if (distanceToPlayer <= _ChaseRadius)
            {
                _AiCharacterControl.SetTarget(_Player.transform);
            }
            else
            {
                _AiCharacterControl.SetTarget(_AiCharacterControl.transform);
            }
        }



        private void FireProjectile()
        {
            GameObject newProjectile = Instantiate(_ProjectileToUse, _ProjectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(_DamagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (_Player.transform.position + _AimOffset - _ProjectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _AttackRadius);

            // Draw move sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _ChaseRadius);

        }
    }
}

