using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Companion : MonoBehaviour {
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chaseRadius = 9f;

    [SerializeField] float healingRadius = 9f;
    [SerializeField] float healingPerShot = -30f;
    [SerializeField] float secondsBetweenShots = 0.5f;

    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

    float currentHealthPoints;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;
    bool isHealing = false;

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
        if (distanceToPlayer <= healingRadius && !isHealing)
        {
            isHealing = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots);
        }

        if (distanceToPlayer > healingRadius)
        {
            isHealing = false;
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


    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0) { Destroy(gameObject); }
    }

    private void SpawnProjectile()
    {
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        projectileComponent.SetDamage(healingPerShot);

        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    private void OnDrawGizmos()
    {
        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, healingRadius);

        // Draw move sphere
        Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

    }
}
