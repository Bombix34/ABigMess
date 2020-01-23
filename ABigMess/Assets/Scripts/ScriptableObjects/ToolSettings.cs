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

    public IEnumerator ApplyEvent(InteractObject objConcerned)
    {
        if(interactionsList.Count == 0)
        {
            yield return null;
        }
        for(int index=0; index<interactionsList.Count;index++)
        {
            if(interactionsList[index].objectConcerned==objConcerned.Settings.objectType)
            {
                yield return interactionsList[index].LaunchEvents(objConcerned.gameObject);
            }
        }
    }
}
