using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Grow Event")]
public class GrowEvent : InteractEvent
{
    [SerializeField]
    private float scaleUpTime = 0.5f;
    private Ease scaleUpEase = Ease.InElastic;

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        objConcerned.GetComponent<Grown>().ScaleUp(scaleUpTime, scaleUpEase);
    }
}
