using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] AnimationClip dyingAnimation;
        [SerializeField] AnimationClip idleAnimation;
        [SerializeField] AnimationClip runAnimation;
        [SerializeField] float minTimeBetweenHit = .5f;
        [SerializeField] float maxAttackRange = 2f;

        public float GetMinTimeBetweenHit()
        {
            return minTimeBetweenHit; 
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            attackAnimation.events = new AnimationEvent[0];
            return attackAnimation;
        }
        public AnimationClip GetDyingAnimClip()
        {
            dyingAnimation.events = new AnimationEvent[0];
            return dyingAnimation;
        }
        public AnimationClip GetIdleAnimClip()
        {
            idleAnimation.events = new AnimationEvent[0];
            return idleAnimation;
        }
        public AnimationClip GetRunAnimClip()
        {
            runAnimation.events = new AnimationEvent[0];
            return runAnimation;
        }
    }
}

