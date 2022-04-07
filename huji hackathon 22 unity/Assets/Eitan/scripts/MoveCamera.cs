using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] 
    private Camera cam;

    [SerializeField] 
    private float zoomStep, minCamSize, maxCamSize;

    [SerializeField] 
    private SpriteRenderer mapRenderer;

    private float mapMinX, mapMaxX, mapMinY, mapMaxY;
    private bool movementAllowed = false;
    
    private Vector3 dragOrigin;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    [SerializeField]
    float zoomModifierSpeed = 0.1f;
    
    private void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            PinchZoom();
        }
        else
        {
            PanCamera();
        }

    }
    
    private void PanCamera()
    {
        // save position of mouse in world space when drag starts (first time clicked)
        if (DetectBGDrag.mouseDown)
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            DetectBGDrag.mouseDown = false;
            DetectBGDrag.AllowBgMovement = true;
        }
        
        // calculate distance between drag origin and new position if it is still held down
        if (DetectBGDrag.AllowBgMovement)
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        
            // move the camera by that distance
            cam.transform.position = ClampCamera(cam.transform.position) + difference;
        }
    }
    
    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
    
    private void PinchZoom()
    {
        if (Input.touchCount == 2) {
            Touch firstTouch = Input.GetTouch (0);
            Touch secondTouch = Input.GetTouch (1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                cam.orthographicSize += zoomModifier/10;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                cam.orthographicSize -= zoomModifier/10;
			
        }

        cam.orthographicSize = Mathf.Clamp (cam.orthographicSize, 5f, 100f);
    }
}
