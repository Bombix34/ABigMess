using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Plug Event")]
public class PlugEvent : InteractEvent
{
    [SerializeField]
    ParticleSystem bolts;

    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Plugging " + objConcerned);
        SetupObjectState(objConcerned);
        ParticleSystem particles = Instantiate(bolts, objConcerned.transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 1f);
    }
}
