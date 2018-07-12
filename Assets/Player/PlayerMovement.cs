using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{   
    ThirdPersonCharacter thirdPersonCharacter = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    Vector3 currentDestination, clickPoint;
    AICharacterControl aiCharacterControl = null; 
    GameObject walkTarget = null;

    [SerializeField] const int walkableLayer = 9;
    [SerializeField] const int enemyLayer = 10;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
        aiCharacterControl = GetComponent<AICharacterControl>();
        walkTarget = new GameObject("walkTarget");
        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick; 
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit)
    {
        switch(layerHit)
        {
            case enemyLayer:
                GameObject enemy = raycastHit.collider.gameObject;
                aiCharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayer:
                walkTarget.transform.position = raycastHit.point;
                aiCharacterControl.SetTarget(walkTarget.transform);
                break;            
            default:
                Debug.LogWarning("Don't know how to handle mouse click for player movement"); 
                break;
        }
    }

    // TODO Make this work again
    private void ProcessGamePadMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
    }

  

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, .1f);
        Gizmos.DrawSphere(clickPoint, .2f);

        Gizmos.color = new Color(255f, 255f, 255f, 0.5f);
    }
}

