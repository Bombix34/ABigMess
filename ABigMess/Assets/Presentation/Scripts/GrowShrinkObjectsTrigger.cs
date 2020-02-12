using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GrowShrinkObjectsTrigger : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToGrow;

    public float timerToGrow = 1f;

    public bool grow;

    private Vector3 almostZero = new Vector3(0.0001f, 0.0001f, 0.0001f);

    void Start()
    {
        foreach(GameObject o in objectsToGrow)
        {
            if (grow)
            {
                o.transform.localScale = almostZero;
            } else
            {
                o.transform.localScale = Vector3.one;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject o in objectsToGrow)
            {
                if (grow)
                {
                    o.transform.DOScale(Vector3.one, timerToGrow);
                } else
                {
                    o.transform.DOScale(almostZero, timerToGrow);
                }
            }
        }
    }
}
