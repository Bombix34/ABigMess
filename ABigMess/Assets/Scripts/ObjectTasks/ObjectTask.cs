using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object tasks")]
[Serializable]
public class ObjectTask : ScriptableObject
{
    [Header("Task")]
    public string description;
    public string destination;
    public int count = 1;
    public ObjectSettings.ObjectType objectType;

    public List<Sprite> taskIcons;

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
