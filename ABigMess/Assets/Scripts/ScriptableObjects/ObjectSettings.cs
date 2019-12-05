using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object settings")]
public class ObjectSettings : ScriptableObject
{
    public ObjectType objectType;
    public ObjectWeight weightType;

  //  public ObjectPriorityList priorityList;

    public Vector3 rotation;

    public bool IsTool()
    {
        return objectType == ObjectType.brush || objectType == ObjectType.freeHand;
    }
    
    public enum ObjectWeight
    {
        light,  // basic object
        medium, // object that slow you down
        heavy   // object that need 2 players to be bringed
    }

    public enum ObjectType
    {
        freeHand,
        brush,
        frigde,
        radio
    }
}
