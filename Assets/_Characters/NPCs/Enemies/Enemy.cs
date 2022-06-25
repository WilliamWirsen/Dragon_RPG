using RPG.Weapons;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters.NPCs.Enemies
{
    [RequireComponent(typeof(NonControllableCharacter))]
    public class Enemy : MonoBehaviour
    {

        [SerializeField] float chaseRadius = 7f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float secondsBetweenShots = 0.5f;

        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] TextMesh _damageText;
        
        AICharacterControl aiCharacterControl = null;
        GameObject player = null;
        bool isAttacking = false;

        
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            aiCharacterControl = GetComponent<AICharacterControl>();
        }

        private void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, aiCharacterControl.transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                InvokeRepeating(nameof(FireProjectile), 0f, secondsBetweenShots);
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

