using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TestCollideEvent : MonoBehaviour
{
    [SerializeField] UnityEvent collideEvent;

    public void OnCollisionEnter(Collision collision)
    {
        collideEvent.Invoke();
    }
}
