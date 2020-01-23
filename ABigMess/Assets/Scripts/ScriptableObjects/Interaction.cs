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

    public IEnumerator LaunchEvents(GameObject obj)
    {
        for(int index=0; index<eventsToLaunch.Count;index++)
        {
            if (eventsToLaunch[index] is TimerEvent)
            {
                TimerEvent timerEvent = (TimerEvent)eventsToLaunch[index];
                Debug.Log("Timer Event : " + timerEvent);
                yield return new WaitForSeconds(timerEvent.delay);
            }
            else
            {
                eventsToLaunch[index].InteractionEvent(obj);
                yield return null;
            }
        }
    }



}

