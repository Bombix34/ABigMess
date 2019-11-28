using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Interaction
{
    public ObjectSettings.ObjectType objectConcerned;


    public List<InteractEvent> eventsToLaunch;

    public void LaunchEvents(GameObject obj)
    {
        for(int index=0; index<eventsToLaunch.Count;index++)
        {
            eventsToLaunch[index].InteractionEvent(obj);
        }
    }

}

