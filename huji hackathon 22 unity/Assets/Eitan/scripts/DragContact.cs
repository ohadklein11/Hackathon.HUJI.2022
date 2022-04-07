using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragContact : MonoBehaviour
{
    private bool moveAllowed;
    private Collider2D col;

    private Vector2 touchBegin;
    private Vector2 touchEnd;

    private GameObject editPopUp;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        editPopUp = GameObject.Find("Canvas").transform.Find("")
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            
            if (touch.phase == TouchPhase.Began)
            {
                Collider2D touchColl = Physics2D.OverlapPoint(touchPosition);
                if (col == touchColl)
                {
                    moveAllowed = true;
                    touchBegin = touchPosition;
                    // print(touchBegin);
                }
            }
            
            if (touch.phase == TouchPhase.Moved)
            {
                if (moveAllowed)
                {
                    transform.position = new Vector2(touchPosition.x, touchPosition.y);
                }
            }
            
            if (touch.phase == TouchPhase.Ended)
            {
                moveAllowed = false;
                touchEnd = touchPosition;
                // print(touchEnd);
                // print(Vector2.Distance(touchBegin, touchEnd));
            }
            
            // detect click
            if (touchBegin == touchEnd)
            {
                print("click");
                editPopUp.SetActive(true);
            }
            
        }
    }
}
