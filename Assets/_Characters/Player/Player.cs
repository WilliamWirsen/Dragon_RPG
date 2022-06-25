using RPG.CameraUI;
using RPG.Characters.NPCs;
using RPG.Core;
using RPG.Weapons;
using UnityEngine;
using UnityEngine.Assertions;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {

        [SerializeField] int enemyLayer = 10;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;

        [SerializeField] Weapon weaponInUse;
        [SerializeField] AnimatorOverrideController animatorOverrideController;
        [SerializeField] GameObject _popupText;


        Animator animator;
        float currentHealth;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        public float HealthAsPercentage
        {
            get
            {
                float healthAsPercentage = currentHealth / maxHealthPoints;
                return healthAsPercentage;
            }

        }

        void Start()
        {
            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand();
            SetupRuntimeAnimator();
        }

        public void TakeDamage(float damage)
        {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealthPoints);
            ScreenPopupText.CreateDamagePopup(gameObject.transform.position, (int)System.Math.Ceiling(damage), _popupText);
        }

        private void SetCurrentMaxHealth()
        {
            currentHealth = maxHealthPoints;
        }

        private void SetupRuntimeAnimator()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT ATTACK"] = weaponInUse.GetAttackAnimClip(); // TODO remove const
            animatorOverrideController["HumanoidIdle"] = weaponInUse.GetIdleAnimClip();
            animatorOverrideController["HumanoidRun"] = weaponInUse.GetRunAnimClip();
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
            if (layerHit == enemyLayer)
            {
                var enemy = raycastHit.collider.gameObject;

                if (IsTargetInRange(enemy))
                {
                    AttackTarget(enemy);
                }
            }
        }

        private void AttackTarget(GameObject enemy)
        {
            var npcComponent = enemy.GetComponent<NonControllableCharacter>();
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHit())
            {
                transform.LookAt(enemy.transform.position);
                animator.SetTrigger("Attack"); // TODO make const                
                npcComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
        }

        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Debug-draw all contact points and normals
            foreach (ContactPoint contact in collision.contacts)
            {
                Debug.DrawRay(contact.point, contact.normal, Color.white);
            }
        }
    }
}


