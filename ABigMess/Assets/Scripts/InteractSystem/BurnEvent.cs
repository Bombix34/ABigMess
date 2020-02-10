using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Burned Event")]
public class BurnEvent : InteractEvent
{

    [SerializeField] Material material;

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        TryInstantiateParticleFX(objConcerned);

        if(material != null)
        {
            objConcerned.GetComponent<Burnt>().BurnMaterial(material);
        }
    }
}