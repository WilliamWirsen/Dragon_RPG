using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour, IDamageable {

    [SerializeField] int enemyLayer = 10;
    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHit = .5f;
    [SerializeField] float maxAttackRange = 2f;

    [SerializeField] Weapon weaponInUse;

    float currentHealth;
    GameObject currentTarget;
    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0f;

    void Start()
    {
        RegisterForMouseClick();
        currentHealth = maxHealthPoints;
        PutWeaponInHand();
        
    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        GameObject dominantHand = RequestDominantHand();
        var weapon = Instantiate(weaponPrefab, dominantHand.transform);
        weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
        weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
    }

    private GameObject RequestDominantHand()
    {
        var dominantHands = GetComponentsInChildren<DominantHand>();
        int numberOfDominantHands = dominantHands.Length;
        Assert.IsFalse(numberOfDominantHands <= 0, "No dominantHand found on Player, please add one");
        Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominantHand scripts on player, please remove one");
        return dominantHands[0].gameObject; 
    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
    }

    void OnMouseClick(RaycastHit raycastHit, int layerHit) 
    {
        if(layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;            

            if((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return; 
            }

            currentTarget = enemy;

            var enemyComponent = enemy.GetComponent<Enemy>();
            if(Time.time - lastHitTime > minTimeBetweenHit)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }            
        }
    }

    public float healthAsPercentage
    {
        get
        {
            float healthAsPercentage = currentHealth / maxHealthPoints;
            return healthAsPercentage;
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealthPoints);        
    }
}
