﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Object tasks")]
[Serializable]
public class ObjectTask : ScriptableObject
{
    public string taskName;
    public EventKeyWord eventType;
    public NumberType numberDesired;
    public int count = 1;
    public ObjectState.State stateConcerned;
    public ObjectSettings.ObjectType objectTypeConcerned;
    public ObjectZoneArea.ZoneAreaType destinationForBring;

    public Sprite destinationSprite;

    private int interfaceCount = 0;

    public List<Sprite> taskIcons;

    public bool IsDone { get; set; }

    public bool IsTaskDone(SceneObjectDatas datas)
    {
        IsDone = false;
        switch(eventType)
        {
            case EventKeyWord.bring:
                List<InteractObject> concernedObject = new List<InteractObject>();
                if (stateConcerned == ObjectState.State.none)
                {
                    concernedObject = datas.GetObjectsOfTypeInZone(objectTypeConcerned, destinationForBring);
                }
                else
                {
                    concernedObject = datas.GetObjectsOfTypeInStateInZone(objectTypeConcerned, stateConcerned, destinationForBring);
                }
                List<InteractObject> totalNumberOfObject = datas.GetObjectsOfType(objectTypeConcerned);
                if (numberDesired == NumberType.number)
                {
                    interfaceCount = count-concernedObject.Count;
                    if (totalNumberOfObject.Count < count)
                    {
                        interfaceCount =totalNumberOfObject.Count - concernedObject.Count;
                        if (concernedObject.Count >= totalNumberOfObject.Count)
                        {
                            IsDone = true;
                        }
                    }
                    else if (concernedObject.Count >= count)
                    {
                        interfaceCount = 0;
                        IsDone = true;
                    }
                }
                else
                {
                    if (concernedObject.Count >= totalNumberOfObject.Count)
                    {
                        IsDone = true;
                    }
                }

                break;
            default:
                concernedObject = new List<InteractObject>();
                if (stateConcerned==ObjectState.State.none)
                {
                    concernedObject = datas.GetObjectsOfType(objectTypeConcerned);
                }
                else
                {
                    concernedObject = datas.GetObjectsOfTypeInState(objectTypeConcerned,stateConcerned);
                }
                if(concernedObject.Count==0)
                {
                    interfaceCount = 0;
                    IsDone = true;
                }
                else
                {
                    List<InteractObject> concernedObjectsInState = datas.GetObjectsOfTypeInState(objectTypeConcerned, GetObjectStateFromEventType());
                    //si on cherche un nombre spécifique
                    if(numberDesired==NumberType.number)
                    {
                        interfaceCount = count-concernedObjectsInState.Count;
                        //si le nombre total d'objet concerné est inférieur au nombre voulu
                        if (concernedObject.Count < count)
                        {
                            interfaceCount = concernedObject.Count-concernedObjectsInState.Count;
                            if (concernedObjectsInState.Count >= concernedObject.Count)
                            {
                                interfaceCount = 0;
                                IsDone = true;
                            }
                        }
                        // si le nombre d'objet dans l'état souhaité est supérieur ou égal
                        else if(concernedObjectsInState.Count>=count)
                        {
                            interfaceCount = 0;
                            IsDone = true;
                        }
                    }
                    //si on cherche tous les objets d'un type
                    else
                    {
                        interfaceCount = concernedObject.Count-concernedObjectsInState.Count;
                        if (concernedObjectsInState.Count >= concernedObject.Count)
                        {
                            interfaceCount = 0;
                            IsDone = true;
                        }
                    }
                }
                break;
        }
        return IsDone;
    }

    private ObjectState.State GetObjectStateFromEventType()
    {
        ObjectState.State toReturn = ObjectState.State.none;
        switch(eventType)
        {
            case EventKeyWord.bring:
                break;
            case EventKeyWord.color:
                toReturn = ObjectState.State.colored;
                break;
            case EventKeyWord.wash:
                toReturn = ObjectState.State.washed;
                break;
            case EventKeyWord.burn:
                toReturn = ObjectState.State.burnt;
                break;
            case EventKeyWord.cook:
                toReturn = ObjectState.State.cooked;
                break;
            case EventKeyWord.plug:
                toReturn = ObjectState.State.plugged;
                break;
            case EventKeyWord.grow:
                toReturn = ObjectState.State.grown;
                break;
            case EventKeyWord.shrink:
                toReturn = ObjectState.State.grown;
                break;
        }
        return toReturn;
    }

    public int GetCountForInterface()
    {
        return interfaceCount;
    }

    public enum EventKeyWord
    {
        bring,
        color,
        wash,
        burn,
        cook,
        plug,
        grow,
        shrink
    }

    public enum NumberType
    {
        all,
        number
    }

}
