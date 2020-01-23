using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            InteractObject otherIO = other.gameObject.GetComponent<InteractObject>();
            print(this.transform.parent.gameObject + " is the plug for " + other.name + " that needs to be plugged ?: " + otherIO.Settings.NeedsToBePlugged());
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();


            Plugged plugged = other.gameObject.GetComponent<Plugged>();

            if (otherIO.Settings.NeedsToBePlugged())
            {
                //Debug.Log("Object Type : " + interactObject.Settings.objectType);
                if (objectState != null)
                {
                    objectState.Plugged = true;
                }
                if (plugged != null)
                {
                    plugged.plug = transform.parent.gameObject;
                }
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();

            if (objectState != null)
            {
                objectState.Plugged = false;
            }
        }
    }
}
