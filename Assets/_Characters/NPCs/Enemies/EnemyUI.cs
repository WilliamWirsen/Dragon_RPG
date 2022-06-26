﻿using UnityEngine;


namespace RPG.Characters.NPCs.Enemies
{
    // Add a UI Socket transform to your enemy
    // Attack this script to the socket
    // Link to a canvas prefab that contains NPC UI
    public class EnemyUI : MonoBehaviour
    {

        // Works around Unity 5.5's lack of nested prefabs
        [Tooltip("The UI canvas prefab")]
        [SerializeField]
        GameObject enemyCanvasPrefab = null;

        Camera cameraToLookAt;
        NonControllableCharacter _NonControllableCharacter = null;
        // Use this for initialization 
        void Start()
        {
            _NonControllableCharacter = GetComponentInParent<NonControllableCharacter>();
            cameraToLookAt = Camera.main;
            Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
        }

        // Update is called once per frame 
        void LateUpdate()
        {
            if (_NonControllableCharacter != null && _NonControllableCharacter.IsAlive() == false)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.LookAt(cameraToLookAt.transform);
            }
        }
    }
}
