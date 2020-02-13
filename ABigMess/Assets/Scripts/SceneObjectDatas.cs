using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneObjectDatas : Singleton<SceneObjectDatas>
{
    private List<InteractObject> objectsInScene;
    private List<ObjectZoneArea> areasInScene;

    protected override void Awake()
    {
        base.Awake();
        objectsInScene = new List<InteractObject>();
        areasInScene = new List<ObjectZoneArea>();
    }

    public void AddObject(InteractObject obj)
    {
        objectsInScene.Add(obj);
    }

    public void RemoveObject(InteractObject obj)
    {
        objectsInScene.Remove(obj);
    }

    public void AddZoneArea(ObjectZoneArea area)
    {
        areasInScene.Add(area);
    }

    public void RemoveZoneArea(ObjectZoneArea area)
    {
        areasInScene.Remove(area);
    }

    public string ObjectsToString()
    {
        string returnVal = "";
        foreach(var obj in objectsInScene)
        {
            char[] toChar = obj.ToString().ToCharArray();
            string final = "";
            char prevChar = ' ';
            foreach(char c in toChar)
            {
                if(prevChar==' '&& c=='(')
                {
                    break;
                }
                final += c;
                prevChar = c;
            }
            returnVal += final + "\n";
        }
        return returnVal;
    }

    public List<InteractObject> GetObjectsOfTypeInState(ObjectSettings.ObjectType objectType, ObjectState.State objectState)
    {
        List<InteractObject> toReturn = GetObjectsOfType(objectType);
        List<InteractObject> statesObject = GetObjectsInState(objectState, toReturn);
        return statesObject;
    }


    public List<InteractObject> GetObjectsOfTypeInState(ObjectSettings.ObjectType objectType, ObjectState.State objectState, List<InteractObject> objList)
    {
        List<InteractObject> toReturn = GetObjectsOfType(objectType, objList);
        List<InteractObject> statesObject = GetObjectsInState(objectState, toReturn);
        return statesObject;
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
    public List<InteractObject> GetObjectsOfType(ObjectSettings.ObjectType objectType, List<InteractObject> objList)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        foreach (var obj in objList)
        {
            if (obj.Settings != null && obj.Settings.objectType == objectType)
            {
                toReturn.Add(obj);
            }
        }
        return toReturn;
    }

    /// <summary>
    /// Get interacted objects in "objectState" from all objects in scene 
    /// </summary>
    /// <param name="objectState"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get interacted objects in "objectState" from a list 
    /// </summary>
    /// <param name="objectState"></param>
    /// <param name="objList"></param>
    /// <returns></returns>
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

    public List<InteractObject> GetObjectsOfTypeInZone(ObjectSettings.ObjectType objectType, ObjectZoneArea.ZoneAreaType objectZoneArea)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        List<ObjectZoneArea> objectZone = GetZoneOfType(objectZoneArea);
        foreach (ObjectZoneArea zone in objectZone)
        {
            List<InteractObject> concernedToAdd = GetObjectsOfType(objectType, zone.GetObjectsInZone());
            foreach(var item in concernedToAdd)
            {
                if(!toReturn.Contains(item))
                {
                    toReturn.Add(item);
                }
            }
        }
        return toReturn;
    }

    public List<InteractObject> GetObjectsOfTypeInStateInZone(ObjectSettings.ObjectType objectType, ObjectState.State objectState, ObjectZoneArea.ZoneAreaType objectZoneArea)
    {
        List<InteractObject> toReturn = new List<InteractObject>();
        List<ObjectZoneArea> objectZone = GetZoneOfType(objectZoneArea);
        foreach (ObjectZoneArea zone in objectZone)
        {
            List<InteractObject> concernedToAdd = GetObjectsOfTypeInState(objectType, objectState, zone.GetObjectsInZone());
            foreach (var item in concernedToAdd)
            {
                if (!toReturn.Contains(item))
                {
                    toReturn.Add(item);
                }
            }
        }
        return toReturn;
    }

    public List<ObjectZoneArea> GetZoneOfType(ObjectZoneArea.ZoneAreaType areaType)
    {
        List<ObjectZoneArea> toReturn = new List<ObjectZoneArea>();
        foreach(ObjectZoneArea area in areasInScene)
        {
            if(area.AreaType == areaType)
            {
                toReturn.Add(area);
            }
        }
        return toReturn;
    }

}
