using System;
using System.Collections;
using System.Collections.Generic;
using Team.Ethan;
using UnityEngine;
using UnityEngine.UI;

public class DragContact : MonoBehaviour
{
    private Contact contact;
    
    private bool moveAllowed;
    private Collider2D col;

    private Vector2 touchBegin;
    private Vector2 touchEnd;

    public GameObject editPopUp;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
        // editPopUp = GameObject.Find("Canvas").transform.Find("ohadPopUp").gameObject;
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

                // detect click
                if (touchBegin == touchEnd)
                {
                    editPopUp.SetActive(true);
                    // send Contact to popUp object
                    // editPopUp.GetComponent<Team.Ethan.ContactView>().ContactInitializer();
                }
            }
        }
    }


}
