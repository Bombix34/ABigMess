using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS/Object priority list")]
public class ObjectPriorityList :ScriptableObject
{
    public List<ObjectSettings.ObjectType> priorityList; 
}
