using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectDatas : MonoBehaviour
{
    [SerializeField]
    private List<InteractObject> objectsInScene;

    public List<InteractObject> GetObjectsOfTypeInState(ObjectSettings.ObjectType objectType, ObjectState.State objectState)
    {
        List<InteractObject> toReturn = GetObjectsOfType(objectType);
        List<InteractObject> statesObject = GetObjectsInState(objectState, toReturn);
        foreach(var obj in statesObject)
        {
            toReturn.Add(obj);
        }
        return toReturn;
    }

    public List<InteractObject> GetObjectsOfType(ObjectSettings.ObjectType objectType)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        foreach (var obj in objectsInScene)
        {
            if (obj.Settings != null && obj.Settings.objectType == objectType)
            {
                toReturn.Add(obj);
            }
        }
        return toReturn;
    }

    public List<InteractObject> GetObjectsInState(ObjectState.State objectState)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        foreach (var obj in objectsInScene)
        {
            ObjectState curState = obj.GetComponent<ObjectState>();
            if (curState != null)
            {
                switch(objectState)
                {
                    case ObjectState.State.broken:
                        if(curState.Broken)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.burnt:
                        if (curState.Burnt)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.colored:
                        if (curState.Colored)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.cooked:
                        if (curState.Cooked)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.grown:
                        if (curState.Grown)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.opened:
                        if (curState.Opened)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.plugged:
                        if (curState.Plugged)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.smuged:
                        if (curState.Broken)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.washed:
                        if (curState.Washed)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                }
            }
        }
        return toReturn;
    }
    public List<InteractObject> GetObjectsInState(ObjectState.State objectState, List<InteractObject> objList)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        foreach (var obj in objList)
        {
            ObjectState curState = obj.GetComponent<ObjectState>();
            if (curState != null)
            {
                switch (objectState)
                {
                    case ObjectState.State.broken:
                        if (curState.Broken)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.burnt:
                        if (curState.Burnt)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.colored:
                        if (curState.Colored)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.cooked:
                        if (curState.Cooked)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.grown:
                        if (curState.Grown)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.opened:
                        if (curState.Opened)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.plugged:
                        if (curState.Plugged)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.smuged:
                        if (curState.Broken)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                    case ObjectState.State.washed:
                        if (curState.Washed)
                        {
                            toReturn.Add(obj);
                        }
                        break;
                }
            }
        }
        return toReturn;
    }
}
