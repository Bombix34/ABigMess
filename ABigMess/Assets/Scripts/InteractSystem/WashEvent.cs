using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Wash Event")]
public class WashEvent : InteractEvent
{

    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Washing");
        if(MusicManager.Instance!=null)
        {
            MusicManager.Instance.GetSoundManager().UseSpongeSound();
        }
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
    }

}
