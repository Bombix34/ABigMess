using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnPlugEvent : MonoBehaviour
{

    [SerializeField] UnityEvent collisionEnterEvent;
    [SerializeField] UnityEvent collisionExitEvent;
    public Collision collision;

    public void OnCollisionEnter(Collision collision)
    {
        this.collision = collision;
        collisionEnterEvent.Invoke();
    }

    public void OnCollisionExit(Collision collision)
    {
        this.collision = collision;
        collisionExitEvent.Invoke();
    }
}
