using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CameraController : MonoBehaviour {
    GameObject mainCamera;
    [SerializeField] GameObject secondaryCamera;

    bool isMainCameraActive = true;
	// Use this for initialization
	void Start () {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        print(mainCamera);
		
	}
	
	// Update is called once per frame
	void Update () {
        CameraInput();
		
	}

    private void CameraInput()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire1"))
        {
            isMainCameraActive = !isMainCameraActive;
            SwitchCamera();            
        }
    }

    private void SwitchCamera()
    {
        if(isMainCameraActive)
        {
            secondaryCamera.SetActive(false);
            mainCamera.SetActive(true);
        } else
        {
            mainCamera.SetActive(false);
            secondaryCamera.SetActive(true);

        }
        
    }
    
}
