using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIWorldTrigger : MonoBehaviour
{
    public List<RectTransform> toHide;
    public List<RectTransform> toShow;

    private void Start()
    {
        foreach (RectTransform showRect in toShow)
        {
            showRect.DOScale(0f, 0f);
            showRect.DOAnchorPos3DZ(20f, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            foreach(RectTransform rect in toHide)
            {
                rect.DOScale(0f, 0.6f);
            }
            foreach(RectTransform showRect in toShow)
            {
                showRect.DOScale(0.02f, 0.6f);
            }
        }
    }


}
