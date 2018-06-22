using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] float maxHealthPoints = 100f;
    float currentHealth = 100f;

    public float healthAsPercentage
    {
        get
        {
            float healthAsPercentage = currentHealth / maxHealthPoints;
            return healthAsPercentage;
        }

    }
}
