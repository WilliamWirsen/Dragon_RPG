using RPG.Weapons;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters
{
    public class Companion : MonoBehaviour
    {
        [SerializeField]
        float maxHealthPoints = 100f;
        [SerializeField]
        float ChaseRadius = 9f;
        [SerializeField]
        float HealingRadius = 9f;
        [SerializeField]
        float HealingPerShot = -30f;
        [SerializeField]
        float SecondsBetweenShots = 0.5f;
        [SerializeField]
        bool _AlwaysHeal = true;

        [SerializeField]
        GameObject ProjectileToUse;
        [SerializeField]
        GameObject ProjectileSocket;
        [SerializeField]
        Vector3 AimOffset = new(0, 1f, 0);
        [SerializeField]
        CompanionType CompanionType = CompanionType.Healer;

        private float _CurrentHealthPoints;
        private AICharacterControl _AiCharacterControl = null;
        private GameObject _Player = null;
        private bool _IsHealing = false;
        private Player _PlayerComponent = null;
        private bool _IsAlive = true;

        public float HealthAsPercentage
        {
            get
            {
                float healthAsPercentage = _CurrentHealthPoints / maxHealthPoints;
                return healthAsPercentage;
            }
        }
        private void Start()
        {
            _Player = GameObject.FindGameObjectWithTag("Player");
            _PlayerComponent = _Player.GetComponent<Player>();
            _AiCharacterControl = GetComponent<AICharacterControl>();
            _CurrentHealthPoints = maxHealthPoints;
            _IsAlive = true;

            if (CompanionType == CompanionType.Healer)
            {
                _IsHealing = true;
            }
        }

        private void Update()
        {
            float distanceToTarget = Vector3.Distance(_Player.transform.position, _AiCharacterControl.transform.position);
            switch (CompanionType)
            {
                case CompanionType.Healer:
                    HealTarget(distanceToTarget);
                    break;
                default:
                    Debug.LogError($"Companion type not yet implemented: {CompanionType}");
                    break;
            }

            if(_CurrentHealthPoints <= 0)
            {
                _IsAlive = false;
            }

            if (_IsAlive == true)
            {
                ChaseTarget(_Player.transform, distanceToTarget);
            }
        }

        private void ChaseTarget(Transform transform, float distanceToPlayer)
        {
            if (distanceToPlayer <= ChaseRadius)
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
            if(_PlayerComponent.healthAsPercentage <= 0)
            {
                CancelInvoke();
                return;
            }
            if (distanceToTarget <= HealingRadius && !_IsHealing)
            {
                _IsHealing = true;
                InvokeRepeating("SpawnProjectile", 0f, SecondsBetweenShots);
            }
            if (_AlwaysHeal == false)
                return; 
            if (distanceToTarget > HealingRadius || (_PlayerComponent.healthAsPercentage >= 1f || _PlayerComponent.healthAsPercentage <= 0))
            {
                _IsHealing = false;
                CancelInvoke();
            }
        }

        public void TakeDamage(float damage)
        {
            _CurrentHealthPoints = Mathf.Clamp(_CurrentHealthPoints - damage, 0f, maxHealthPoints);
            if (_CurrentHealthPoints <= 0) { Destroy(gameObject); }
        }

        // This method is called by string reference. This is the reason why it doesn't have any references linked.
        private void SpawnProjectile()
        {
            GameObject newProjectile = Instantiate(ProjectileToUse, ProjectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(HealingPerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (_Player.transform.position + AimOffset - ProjectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
            Destroy(newProjectile, 2);
        }

        private void OnDrawGizmos()
        {
            // Draw attack sphere
            Gizmos.color = new Color(255f, 0f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, HealingRadius);

            // Draw move sphere
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, ChaseRadius);

        }
    }
}


