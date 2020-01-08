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
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.GetComponent<InteractObject>() != null)
        {
            if (collisionGameObject.GetComponent<InteractObject>().Settings.objectType == ObjectSettings.ObjectType.plug)
            {
                plug = collisionGameObject;
            }
        }

    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == plug)
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
