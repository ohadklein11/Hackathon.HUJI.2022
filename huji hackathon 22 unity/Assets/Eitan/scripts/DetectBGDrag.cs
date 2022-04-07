using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectBGDrag : MonoBehaviour
{
    public static bool AllowBgMovement = false;
    public static bool mouseDown = false;

    private void OnMouseDown()
    {
        mouseDown = true;
    }

    private void OnMouseUp()
    {
        AllowBgMovement = false;
    }
}
