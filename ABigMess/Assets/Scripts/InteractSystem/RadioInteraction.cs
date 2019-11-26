using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioInteraction : InteractEvent
{
    protected void Start()
    {
        GetComponent<InteractObject>().OnInteractWithoutTool.AddListener(InteractionEvent);    
    }

    protected override void InteractionEvent()
    {
        MusicManager radioManager = GameManager.Instance.MusicManager;
        if(radioManager==null)
        {
            return;
        }
        radioManager.SwitchRadio();
    }
}
