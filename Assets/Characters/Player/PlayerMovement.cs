using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AICharacterControl))]
[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{   
    CameraRaycaster cameraRaycaster = null;
    Vector3 clickPoint;
    AICharacterControl aiCharacterControl = null; 
    GameObject walkTarget = null;

    [SerializeField] const int walkableLayer = 9;
    [SerializeField] const int enemyLayer = 10;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
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
}

