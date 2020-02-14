using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Radio Event")]
public class RadioEvent : InteractEvent
{
    public override void InteractionEvent(GameObject objConcerned)
    {
        MusicManager radioManager = GameManager.Instance.MusicManager;
        if (radioManager == null)
        {
            return;
        }
        radioManager.SwitchRadio();
        if(objConcerned.GetComponent<Animator>()!=null)
        {
            objConcerned.GetComponent<Animator>().SetBool("IsOn", true);
        }
        foreach(ParticleSystem particle in objConcerned.GetComponentsInChildren<ParticleSystem>())
        {
            particle.Play();
        }
        TryInstantiateParticleFX(objConcerned);
    }
}
