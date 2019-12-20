using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectTask 
{
    public GameObject gameObject;
    public Transform desiredPosition;
    public bool done;
    public string description;
}
