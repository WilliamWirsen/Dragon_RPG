using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorAffordance : MonoBehaviour {
    [SerializeField] Texture2D cursorAttack = null;
    [SerializeField] Texture2D cursorQuestionMark = null;
    [SerializeField] Texture2D cursorWalk = null;
    [SerializeField] Vector2 cursorOffset = new Vector2(96, 96);

    CameraRaycaster cameraRaycaster;
    // Use this for initialization
    void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Fire1")) {
            print(cameraRaycaster.layerHit);
        }
        ChangeCursorType();
    }

    private void ChangeCursorType()
    {
        switch(cameraRaycaster.layerHit)
        {
            case Layer.Walkable:
                Cursor.SetCursor(cursorWalk, cursorOffset, CursorMode.Auto); 
                break;
            case Layer.Enemy:
                Cursor.SetCursor(cursorAttack, cursorOffset, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(cursorQuestionMark, cursorOffset, CursorMode.Auto);
                break;
            default:
                Debug.LogWarning("Don't know which cursor to use");
                break;
        }
    }
}
