using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnt : MonoBehaviour
{
    Color defaultColor;
    ObjectState objectState;
    
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        objectState = GetComponent<ObjectState>();
    }

    void Update()
    {
        if (objectState.Burnt)
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
        else
        {
            GetComponent<Renderer>().material.color = defaultColor;
        }
    }
}
