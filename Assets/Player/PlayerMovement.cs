using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    float walkMoveStopRadius = 0.2f;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // todo allow player to re - map later
        if (Input.GetKeyDown(KeyCode.G)) // G for gamepad. 
        {
            isInDirectMode = !isInDirectMode;
            currentClickTarget = transform.position; // clear the clicktarget
            
        }

        if(isInDirectMode)
        {
            ProcessGamePadMovement();
        } else
        {
            ProcessMouseMovement();
        }
        

    }

    private void ProcessGamePadMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:

        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        thirdPersonCharacter.Move(currentClickTarget - transform.position, false, false);
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0)) // Moving by holding down mousebutton
        {
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;             
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
        if (playerToClickedPoint.magnitude >= walkMoveStopRadius)
        {
            thirdPersonCharacter.Move(currentClickTarget - transform.position, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }
}

