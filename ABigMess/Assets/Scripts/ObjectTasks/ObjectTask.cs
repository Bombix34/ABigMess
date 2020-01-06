using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object tasks")]
[Serializable]
public class ObjectTask : ScriptableObject
{
    [Header("Task")]
    public bool done;
    public string description;
    public ObjectSettings.ObjectType objectType;

    [Header("States of the object")]
    [SerializeField] public BoolPair washed;
    [SerializeField] public BoolPair burnt;
    [SerializeField] public BoolPair smuged;
    [SerializeField] public BoolPair cooked;
    [SerializeField] public BoolPair grown;
    [SerializeField] public BoolPair colored;
    [SerializeField] public BoolPair broken;
    [SerializeField] public BoolPair opened;
    [SerializeField] public BoolPair plugged;

}
