using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Change Color Event")]
public class ChangeColorEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
    }
}