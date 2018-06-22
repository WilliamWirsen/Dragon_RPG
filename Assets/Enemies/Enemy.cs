using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] float maxHealthPoints = 100f;
    float currentHealthPoints = 100f;

    public float healthAsPercentage
    {
        get
        {
            float healthAsPercentage = currentHealthPoints / maxHealthPoints;
            return healthAsPercentage; 
        }
    }
}
