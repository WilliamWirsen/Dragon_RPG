using RPG.Characters.NPCs.Interfaces;
using RPG.Core;
using UnityEngine;

namespace RPG.Characters.NPCs
{
    public class NonControllableCharacter : MonoBehaviour, INonControllableCharacter, IDamageable
    {
        [SerializeField]
        private float _MaxHealthPoints = 100f;
        [SerializeField]
        private GameObject _popupTextGameObject;

        public float CurrentHealthPoints { get; private set; }
        public bool IsAlive { get; private set; }
        public float HealthAsPercentage
        {
            get => CurrentHealthPoints / _MaxHealthPoints;
        }

        public void Start()
        {
            IsAlive = true;
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
            IsAlive = false;
        }
    }
}
