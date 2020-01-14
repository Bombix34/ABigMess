using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS/Tool settings")]
[Serializable]
public class ToolSettings : ObjectSettings
{
    [HideInInspector]
    public List<Interaction> interactionsList;

    public void ApplyEvent(InteractObject objConcerned)
    {
        if(interactionsList.Count == 0)
        {
            return;
        }
        for(int index=0; index<interactionsList.Count;index++)
        {
            if(interactionsList[index].objectConcerned==objConcerned.Settings.objectType)
            {
                interactionsList[index].LaunchEvents(objConcerned.gameObject);
            }
        }
    }
}
