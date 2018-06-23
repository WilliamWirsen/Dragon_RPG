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
    [SerializeField] const int walkableLayer = 9;
    [SerializeField] const int enemyLayer = 10;

    CameraRaycaster cameraRaycaster;

    // Use this for initialization
    void Start () {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; // registrate
    }

    private void OnLayerChanged(int newLayer)
    {
        switch(newLayer)
        {
            case walkableLayer:
                Cursor.SetCursor(cursorWalk, cursorOffset, CursorMode.Auto); 
                break;
            case enemyLayer:
                Cursor.SetCursor(cursorAttack, cursorOffset, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(cursorQuestionMark, cursorOffset, CursorMode.Auto);
                break;
        }
    }
}
