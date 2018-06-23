using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackMoveStopRadius = 5f;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    
    private void ProcessGamePadMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
    }

    //private void ProcessMouseMovement()
    //{
    //    if (Input.GetMouseButton(0)) // Moving by holding down mousebutton
    //    {
    //        clickPoint = cameraRaycaster.hit.point;
    //        switch (cameraRaycaster.layerHit)
    //        {
    //            case Layer.Walkable:
    //                currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
    //                break;
    //            case Layer.Enemy:
    //                currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
    //                // do stuff
    //                break;
    //            default:
    //                Debug.Log("No passable surface hit.");
    //                break;
    //        }
    //    }
    //    WalkToDestination();
    //}

    private void WalkToDestination()
    {
        var playerToClickedPoint = transform.position - currentDestination;
        if (playerToClickedPoint.magnitude >= 0)
        {
            thirdPersonCharacter.Move(currentDestination - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector; 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, .1f);
        Gizmos.DrawSphere(clickPoint, .2f);

        Gizmos.color = new Color(255f, 255f, 255f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    }
}

