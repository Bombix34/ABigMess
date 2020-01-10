using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollideEvent : MonoBehaviour
{
    [SerializeField] UnityEvent collideEvent;
    public Collision collision;


    public void OnCollisionEnter(Collision collision)
    {
        this.collision = collision;
        collideEvent.Invoke();
    }
}
