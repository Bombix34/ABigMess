using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Plug Event")]
public class PlugEvent : InteractEvent
{
    [SerializeField] Ease ease;

    [SerializeField] GameObject bolt;
    
    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Plugging " + objConcerned);
        SetupObjectState(objConcerned);
        GameObject particles = TryInstantiateParticleFX(objConcerned);
        if(particles!=null)
        {
            particles.transform.DOMoveY(2, 0.5f);
            particles.GetComponent<Renderer>().material.DOFade(0, 0.5f).SetEase(ease);
        }

        // show a little particle when plugged
        if (bolt != null)
        {
            Debug.Log("hello i'm showing a bolt pliz");
            GameObject boltInstance = Instantiate(bolt, objConcerned.transform);
            boltInstance.transform.DOMoveY(2, 0.5f);
            boltInstance.GetComponent<Renderer>().material.DOFade(0, 0.5f).SetEase(ease);
        }
    }
}
