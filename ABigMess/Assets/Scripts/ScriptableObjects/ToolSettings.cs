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

    [SerializeField]
    private Sprite actionIcon;

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

    public bool IsInteractionExisting(ObjectType objectType)
    {
        bool toReturn = false;
        foreach(Interaction interaction in interactionsList)
        {
            if(interaction.objectConcerned==objectType)
            {
                toReturn = true;
            }
        }
        return toReturn;
    }

    public Sprite ActionIcon
    {
        get => actionIcon;
    }
}
