using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Burned Event")]
public class BurnEvent : InteractEvent
{

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
    }
}