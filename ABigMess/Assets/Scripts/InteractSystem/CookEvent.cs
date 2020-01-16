using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Cook Event")]
public class CookEvent : InteractEvent
{

    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Cooking " + objConcerned);
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
    }

}
