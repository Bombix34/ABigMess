using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Casual animation Event")]
public class CasualAnimationEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        if (objConcerned.GetComponent<Animator>() != null)
        {
            objConcerned.GetComponent<Animator>().SetTrigger("IsOn");
        }
    }

}
