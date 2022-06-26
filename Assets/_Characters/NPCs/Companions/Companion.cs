using RPG.Weapons;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters.NPCs.Companions
{
    [RequireComponent(typeof(NonControllableCharacter))]
    public class Companion : MonoBehaviour
    {
        [SerializeField]
        float _ChaseRadius = 9f;
        [SerializeField]
        float _HealingRadius = 9f;
        [SerializeField]
        float _HealingPerShot = -30f;
        [SerializeField]
        float _SecondsBetweenShots = 0.5f;
        [SerializeField]
        bool _AlwaysHeal = true;

        [SerializeField]
        GameObject _ProjectileToUse;
        [SerializeField]
        GameObject _ProjectileSocket;
        [SerializeField]
        Vector3 _AimOffset = new(0, 1f, 0);
        [SerializeField]
        CompanionType _CompanionType = CompanionType.Healer;

        private AICharacterControl _AiCharacterControl = null;
        private GameObject _Player = null;
        private bool _IsHealing = false;
        private Player _PlayerComponent = null;
        private NonControllableCharacter _NonControllableCharacterComponent = null;

        private void Start()
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            _PlayerComponent = _Player.GetComponent<Player>();
            _AiCharacterControl = GetComponent<AICharacterControl>();
            _NonControllableCharacterComponent = GetComponent<NonControllableCharacter>();

            if (_CompanionType == CompanionType.Healer)
            {
                _IsHealing = true;
            }
        }

        private void Update()
        {
            float distanceToTarget = Vector3.Distance(_Player.transform.position, _AiCharacterControl.transform.position);
            switch (_CompanionType)
            {
                case CompanionType.Healer:
                    HealTarget(distanceToTarget);
                    break;
                default:
                    Debug.LogError($"Companion type not yet implemented: {_CompanionType}");
                    break;
            }            
        }

        private void LateUpdate()
        {
            float distanceToTarget = Vector3.Distance(_Player.transform.position, _AiCharacterControl.transform.position);
            if (_NonControllableCharacterComponent.IsAlive())
            {
                ChaseTarget(_Player.transform, distanceToTarget);
            }
        }

        private void ChaseTarget(Transform transform, float distanceToPlayer)
        {
            if (distanceToPlayer <= _ChaseRadius)
            {
                _AiCharacterControl.SetTarget(_Player.transform);
            }
            else
            {
                _AiCharacterControl.SetTarget(_AiCharacterControl.transform);
            }
        }

        private void HealTarget(float distanceToTarget)
        {
            // Player is dead
            if(_PlayerComponent.HealthAsPercentage <= 0)
            {
                CancelInvoke();
                return;
            }
            if (distanceToTarget <= _HealingRadius && !_IsHealing)
            {
                _IsHealing = true;
                InvokeRepeating(nameof(SpawnProjectile), 0f, _SecondsBetweenShots);
            }
            if (_AlwaysHeal == false)
                return; 
            if (distanceToTarget > _HealingRadius || (_PlayerComponent.HealthAsPercentage >= 1f || _PlayerComponent.HealthAsPercentage <= 0))
            {
                _IsHealing = false;
                CancelInvoke();
            }
        }

        // This method is called by string reference. This is the reason why it doesn't have any references linked.
        private void SpawnProjectile()
        {
            GameObject newProjectile = Instantiate(_ProjectileToUse, _ProjectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(_HealingPerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (_Player.transform.position + _AimOffset - _ProjectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
            Destroy(newProjectile, 2);
        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _HealingRadius);

            // Draw move sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, _ChaseRadius);

        }
    }
}


