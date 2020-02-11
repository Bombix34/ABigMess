using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "BIGMESS_EVENTS/Grow Event")]
public class GrowEvent : InteractEvent
{
    [SerializeField]
    private float scaleUpTime = 0.5f;

    [SerializeField]
    private Ease scaleUpEase = Ease.InElastic;

    [SerializeField]
    private float scaleAdd = 0.125f;

    public override void InteractionEvent(GameObject objConcerned)
    {
        SetupObjectState(objConcerned);
        objConcerned.GetComponent<Grown>().ScaleUp(scaleAdd, scaleUpTime, scaleUpEase);
    }
}
