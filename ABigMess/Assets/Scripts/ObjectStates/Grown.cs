using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grown : MonoBehaviour
{
    public Vector3 initialScale;
    public int maxTimesScaled = 5;
    public int timesScaled = 0;

    private Vector3 lastScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        
    }

    public void ScaleUp(float scaleUp, Ease eases)
    {
        if (timesScaled < maxTimesScaled)
        {
            timesScaled++;
            // Debug.Log( Mathf.Log(25 - timesScaled) - 2);
            // The more I grow the harder it becomes to grow
            transform.DOScale(transform.localScale * (Mathf.Log(26 - timesScaled) - 2f), scaleUp).SetEase(ease);
            lastScale = transform.localScale;
        } else
        {
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(transform.DOScale(lastScale, scaleUp/4));
            scaleSequence.Append(transform.DOScale(lastScale * (Mathf.Log(21) - 2f), scaleUp - (scaleUp/4)).SetEase(ease));
        }
    }

    public void OnDestroy()
    {
        transform.localScale = initialScale;
    }
}
