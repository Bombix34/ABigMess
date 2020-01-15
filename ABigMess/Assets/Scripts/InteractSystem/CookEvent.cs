using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Cook Event")]
public class CookEvent : InteractEvent
{
    [SerializeField]
    ParticleSystem smoke;

    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Cooking " + objConcerned);
        SetupObjectState(objConcerned);
        ParticleSystem particles = Instantiate(smoke, objConcerned.transform.position, Quaternion.identity);
        Destroy(particles.gameObject, 0.25f);
    }

}
