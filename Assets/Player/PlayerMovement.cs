using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    float walkMoveStopRadius = 0.2f;
        
    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    { 
        if (Input.GetMouseButton(0)) // Moving by holding down mousebutton
        {
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;  // So not set in default case                    
                    
                    break;
                case Layer.Enemy:
                    print("Enemy hit");
                    // do stuff
                    break;
                default:
                    Debug.Log("No passable surface hit.");
                    break;
            }
        }
        var playerToClickedPoint = transform.position - currentClickTarget;
        if(playerToClickedPoint.magnitude >= walkMoveStopRadius)
        {
            m_Character.Move(currentClickTarget - transform.position, false, false);
        } else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
        
    }
}

