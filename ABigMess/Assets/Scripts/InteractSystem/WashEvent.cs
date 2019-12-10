using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Wash Event")]
public class WashEvent : InteractEvent
{
    [SerializeField]
    ParticleSystem bubbles;

    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Washing");
        SetupObjectState(objConcerned);
        ParticleSystem particles = Instantiate(bubbles, objConcerned.transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 0.25f);
    }

}
