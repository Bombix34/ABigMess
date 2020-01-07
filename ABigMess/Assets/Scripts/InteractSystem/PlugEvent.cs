using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Plug Event")]
public class PlugEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Plugging " + objConcerned);
        SetupObjectState(objConcerned);
    }
}
