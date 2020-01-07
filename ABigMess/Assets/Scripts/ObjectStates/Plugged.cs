using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plugged : MonoBehaviour
{
    public GameObject plug;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        plug = collision.gameObject;
    }

    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject == plug)
        {
            // Unplug
            gameObject.GetComponent<ObjectState>().Plugged = false;
            Destroy(this);
        }
    }
    public void OnDestroy()
    {
       
    }
}
