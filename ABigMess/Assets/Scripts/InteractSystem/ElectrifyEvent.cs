using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Electrify Event")]
public class ElectrifyEvent : InteractEvent
{

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
    }
}