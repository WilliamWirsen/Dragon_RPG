using RPG.Characters;
using RPG.Characters.NPCs;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        // the navmesh agent required for the path finding
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }
        // the character we are controlling
        public ThirdPersonCharacter character { get; private set; }
        // target to aim for
        public Transform target;

        private Player _Player = null;
        private NonControllableCharacter _NonControllableCharacter = null;
        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            _NonControllableCharacter = GetComponent<NonControllableCharacter>();
            _Player = GetComponent<Player>();

            agent.updateRotation = false;
            agent.updatePosition = true;
        }


        private void Update()
        {
            // Character is dead
            if((_Player != null && _Player.CurrentHealth <= 0) ||  _NonControllableCharacter != null && _NonControllableCharacter.CurrentHealthPoints <= 0)
            {
                return;
            }

            if (target != null)
                agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);

        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }
}
