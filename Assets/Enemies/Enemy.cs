using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float attackRadius = 4f;
    [SerializeField] float moveRadius = 10f;

    float currentHealthPoints = 100f;
    AICharacterControl aiCharacterControl = null;
    GameObject player = null;

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
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, aiCharacterControl.transform.position); 
        if(distanceToPlayer <= attackRadius)
        {
            print(gameObject.name + " attacking player");
            // TODO: Spawn projectile
        }

        if (distanceToPlayer <= moveRadius)
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
    }

    private void OnDrawGizmos()
    {
        // Draw attack sphere
        Gizmos.color = new Color(255f, 0f, 0, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw move sphere
        Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, moveRadius);
        
    }
}
