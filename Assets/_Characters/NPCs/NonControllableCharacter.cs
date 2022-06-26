using RPG.Characters.NPCs.Interfaces;
using RPG.Core;
using RPG.Weapons;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityStandardAssets.Characters.ThirdPerson;

namespace RPG.Characters.NPCs
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AnimatorOverrideController))]
    public class NonControllableCharacter : MonoBehaviour, INonControllableCharacter, IDamageable
    {
        [SerializeField]
        private float _MaxHealthPoints = 100f;
        [SerializeField]
        private GameObject _popupTextGameObject;
        [SerializeField] 
        Weapon _WeaponInUse;
        [SerializeField]
        private AnimatorOverrideController _AnimatorOverrideController;

        private Animator _Animator;
        private float _LastHitTime = 0f;
        private ThirdPersonCharacter _ThirdPersonCharacter = null;
        private AICharacterControl _AiCharacterControl = null;
        private Collider _Collider = null; 
        public float CurrentHealthPoints { get; private set; }
        public float HealthAsPercentage
        {
            get => CurrentHealthPoints / _MaxHealthPoints;
        }

        public bool IsAlive() => HealthAsPercentage > 0;


        public void Start()
        {
            Setup();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        private void Setup()
        {
            _ThirdPersonCharacter = GetComponent<ThirdPersonCharacter>() ?? gameObject.AddComponent(typeof(ThirdPersonCharacter)) as ThirdPersonCharacter;
            _AiCharacterControl = GetComponent<AICharacterControl>() ?? gameObject.AddComponent(typeof(AICharacterControl)) as AICharacterControl;
            _Collider = GetComponent<CapsuleCollider>() ?? gameObject.AddComponent(typeof(CapsuleCollider)) as CapsuleCollider;
        }

        private void SetupRuntimeAnimator()
        {
            _Animator = GetComponent<Animator>();
            _Animator.runtimeAnimatorController = _AnimatorOverrideController;
            _AnimatorOverrideController["DEFAULT ATTACK"] = _WeaponInUse.GetAttackAnimClip();
            _AnimatorOverrideController["HumanoidIdle"] = _WeaponInUse.GetIdleAnimClip();
            _AnimatorOverrideController["HumanoidRun"] = _WeaponInUse.GetRunAnimClip();
            _AnimatorOverrideController["DEFAULT DIE"] = _WeaponInUse.GetDyingAnimClip();
        }

        private void PutWeaponInHand()
        {
            var weaponPrefab = _WeaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = _WeaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = _WeaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            Assert.IsFalse(numberOfDominantHands <= 0, $"No dominantHand found on {transform.root.name}, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple dominantHand scripts on character, please remove one");
            return dominantHands[0].gameObject;
        }

        private void SetCurrentMaxHealth()
        {
            CurrentHealthPoints = _MaxHealthPoints;
        }

        public void TakeDamage(float damage)
        {
            CurrentHealthPoints = Mathf.Clamp(CurrentHealthPoints - damage, 0f, _MaxHealthPoints);
            ScreenPopupText.CreateDamagePopup(gameObject.transform.position, (int)System.Math.Ceiling(damage), _popupTextGameObject);

            if (CurrentHealthPoints <= 0)
            {
                KillCharacter();
            }
        }

        private void KillCharacter()
        {
            _Collider.enabled = false;
            _Animator.SetTrigger("Die");
            _ThirdPersonCharacter.enabled = false;
            _AiCharacterControl.enabled = false;
        }

        public void AttackTarget(GameObject enemy, float damagePerHit)
        {
            var playerComponent = enemy.GetComponent<Player>();
            if (Time.time - _LastHitTime > _WeaponInUse.GetMinTimeBetweenHit())
            {
                transform.LookAt(enemy.transform.position);
                _Animator.SetTrigger("Attack"); // TODO make const                
                playerComponent.TakeDamage(damagePerHit);
                _LastHitTime = Time.time;
            }
        }
    }
}
