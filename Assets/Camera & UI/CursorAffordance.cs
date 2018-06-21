using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {
    [SerializeField] Texture2D cursorAttack = null;
    [SerializeField] Texture2D cursorQuestionMark = null;
    [SerializeField] Texture2D cursorWalk = null;
    [SerializeField] Vector2 cursorOffset = new Vector2(0, 0);

    CameraRaycaster cameraRaycaster;
    // Use this for initialization
    void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.layerChangeObservers += OnLayerChanged; 
	}

    private void OnLayerChanged()
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
                print("RaycastEndStop");
                Cursor.SetCursor(cursorQuestionMark, cursorOffset, CursorMode.Auto);
                break;
            default:
                Debug.LogWarning("Don't know which cursor to use");
                break;
        }
    }
}
