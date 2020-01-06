using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectTask 
{
    [Header("Object and desired position")]
    public GameObject gameObject;
    public Transform desiredPosition;

    [Header("Task state")]
    public bool done;
    public string description;

    [Header("States of the object")]
    public ObjectStates states;

}
