using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Grow Event")]
public class GrowEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        objConcerned.GetComponent<Grown>().ScaleUp();
    }
}
