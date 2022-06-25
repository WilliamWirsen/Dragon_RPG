using UnityEngine;
using UnityEngine.UI;


namespace RPG.Characters.NPCs.Enemies
{
    public class NonControllableCharacterHealthBar : MonoBehaviour
    {
        RawImage _HealthBarRawImage = null;
        NonControllableCharacter _NpcComponent = null;

        // Use this for initialization
        void Start()
        {
            _NpcComponent = GetComponentInParent<NonControllableCharacter>(); // Different to way player's health bar finds player
            _HealthBarRawImage = GetComponent<RawImage>();
        }

        // Update is called once per frame
        void Update()
        {
            float xValue = -(_NpcComponent.HealthAsPercentage / 2f) - 0.5f;
            _HealthBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }
    }

}
