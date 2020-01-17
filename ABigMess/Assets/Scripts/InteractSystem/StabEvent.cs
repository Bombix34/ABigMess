using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Stab Event")]
public class StabEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
    }
}