using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnt : MonoBehaviour
{
    Color defaultColor;
    Material defaultMaterial;
    ObjectState objectState;
    
    void Start()
    {
        defaultColor = GetComponent<Renderer>().material.color;
        defaultMaterial = GetComponent<Renderer>().material;
        objectState = GetComponent<ObjectState>();

        GetComponent<Renderer>().material.color = Color.black;
    }

    public void BurnMaterial(Material material)
    {
        GetComponent<Renderer>().material.color = defaultColor;
        GetComponent<Renderer>().material = material;
    }

    private void OnDestroy()
    {
        GetComponent<Renderer>().material.color = defaultColor;
        GetComponent<Renderer>().material = defaultMaterial;
    }
}
