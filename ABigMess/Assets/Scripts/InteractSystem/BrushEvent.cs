using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Brush Event")]
public class BrushEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Brushing");
    }
}
