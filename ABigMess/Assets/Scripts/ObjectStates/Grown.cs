using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grown : MonoBehaviour
{

    public Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        
    }

    public void ScaleUp()
    {
        transform.localScale = transform.localScale * 1.125f;
    }

    public void OnDestroy()
    {
        transform.localScale = initialScale;
    }
}
