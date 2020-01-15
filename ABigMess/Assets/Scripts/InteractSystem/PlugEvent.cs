using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Plug Event")]
public class PlugEvent : InteractEvent
{
    [SerializeField] GameObject bolts;
    [SerializeField] Ease ease;
    
    public override void InteractionEvent(GameObject objConcerned)
    {
        Debug.Log("Plugging " + objConcerned);
        SetupObjectState(objConcerned);
        GameObject particles = Instantiate(bolts, objConcerned.transform.position, Quaternion.identity);
        particles.transform.DOMoveY(2, 0.5f);
        particles.GetComponent<Renderer>().material.DOFade(0, 0.5f).SetEase(ease);
        Destroy(particles, 0.5f);
    }
}
