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

    public int speed = 4; 
    public float MINSCALE = 2.0F; 
    public float MAXSCALE = 5.0F; 
    public float minPinchSpeed = 5.0F; 
    public float varianceInDistances = 5.0F; 
    private float touchDelta = 0.0F; 
    private Vector2 prevDist = new Vector2(0,0); 
    private Vector2 curDist = new Vector2(0,0); 
    private float speedTouch0 = 0.0F; 
    private float speedTouch1 = 0.0F;
    
    private void Awake()
    {
        print("awake");
        
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        print("update");
        PanCamera();
    }
    
    private void PanCamera()
    {
        print("methode works");
        print(DetectBGDrag.mouseDown);

        // // save position of mouse in world space when drag starts (first time clicked)
        // if (Input.GetMouseButtonDown(0))
        // {
        //     dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        // }
        
        // // calculate distance between drag origin and new position if it is still held down
        // if (Input.GetMouseButton(0))
        // {
        //     Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        //
        //     // move the camera by that distance
        //     cam.transform.position = ClampCamera(cam.transform.position) + difference;
        // }
        
        // save position of mouse in world space when drag starts (first time clicked)
        if (DetectBGDrag.mouseDown)
        {
            print("allow move");
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            DetectBGDrag.mouseDown = false;
            DetectBGDrag.AllowBgMovement = true;
        }
        
        // calculate distance between drag origin and new position if it is still held down
        if (DetectBGDrag.AllowBgMovement)
        {
            print("move");

            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
        
            // move the camera by that distance
            cam.transform.position = ClampCamera(cam.transform.position) + difference;
        }
        
        // PinchZoom();
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
}
