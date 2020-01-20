using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{

    [SerializeField] UnityEvent collisionEnterEvent;
    [SerializeField] UnityEvent collisionExitEvent;
    public Collider collider;

    public void OnTriggerEnter(Collider other)
    {
        collider = other;
        collisionEnterEvent.Invoke();
    }

    public void OnTriggerExit(Collider other)
    {
        collider = other;
        collisionExitEvent.Invoke();
    }


}
