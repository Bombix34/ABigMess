using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Grown : MonoBehaviour
{
    public Vector3 initialScale;
    public int maxTimesScaled = 5;
    public int timesScaled = 0;

    private Vector3 lastScale;

    public bool animCompleted = true;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        
    }

    public void ScaleUp(float scaleAdd, float scaleUp, Ease ease)
    {
        if (timesScaled < maxTimesScaled && animCompleted)
        {
            timesScaled++;
            // Debug.Log( Mathf.Log(25 - timesScaled) - 2);
            // The more I grow the harder it becomes to grow
            Vector3 sum = transform.localScale;
            sum.x += scaleAdd;
            sum.y += scaleAdd;
            sum.z += scaleAdd;
            transform.DOScale(sum, scaleUp).SetEase(ease).OnComplete(AnimCompleted);
            animCompleted = false;
            lastScale = transform.localScale;
        } else if (animCompleted)
        {
            Sequence scaleSequence = DOTween.Sequence();
            scaleSequence.Append(transform.DOScale(lastScale, scaleUp/4));
            Vector3 sum = lastScale;
            sum.x += scaleAdd;
            sum.y += scaleAdd;
            sum.z += scaleAdd;
            scaleSequence.Append(transform.DOScale(sum, scaleUp - (scaleUp/4)).SetEase(ease));
            scaleSequence.OnComplete(AnimCompleted);
            animCompleted = false;
        }
    }

    private void AnimCompleted()
    {
        animCompleted = true;
    }

    public void OnDestroy()
    {
        transform.localScale = initialScale;
    }
}
