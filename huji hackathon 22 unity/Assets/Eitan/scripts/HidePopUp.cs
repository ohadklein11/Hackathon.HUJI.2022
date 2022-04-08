using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePopUp : MonoBehaviour
{
    private GameObject editPopUp;

    private void Awake()
    {
        editPopUp = GameObject.Find("Canvas").transform.Find("ohadPopUp").gameObject;
    }

    public void HidePoUp()
    {
        editPopUp.SetActive(false);
    }
}
