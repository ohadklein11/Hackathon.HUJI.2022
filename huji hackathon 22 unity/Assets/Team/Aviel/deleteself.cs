using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class deleteself : MonoBehaviour
{
    public GameObject field;
    
    public void Delete()
    {
        field.SetActive(false);
        gameObject.SetActive(false);
    }
}
