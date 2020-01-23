using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvents : MonoBehaviour
{

    [SerializeField] UnityEvent collisionEnterEvent;
    [SerializeField] UnityEvent collisionExitEvent;
    public Collider collider;

    private void Awake()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractObject>() != null)
        {
            collider = other;
            collisionEnterEvent.Invoke();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractObject>() != null)
        {
            collider = other;
            collisionExitEvent.Invoke();
        }
    }


}
