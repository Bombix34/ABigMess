using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Cook Event")]
public class CookEvent : InteractEvent
{
    [SerializeField]
    ParticleSystem cookedFixedParticles;

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);
        if (MusicManager.Instance != null)
        {
            MusicManager.Instance.GetSoundManager().CookSFX();
        }
        objConcerned.GetComponent<Cooked>().CookedParticles(cookedFixedParticles);
    }

}
