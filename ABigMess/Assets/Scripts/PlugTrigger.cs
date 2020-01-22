using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugTrigger : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            //print(this.transform.parent.gameObject + " collided with " + other.name);
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();
            if(objectState != null)
            {
                objectState.Plugged = true;
            }

            InteractObject interactObject = transform.parent.gameObject.GetComponent<InteractObject>();
            Plugged plugged = other.gameObject.GetComponent<Plugged>();

            if (interactObject != null)
            {
                //Debug.Log("Object Type : " + interactObject.Settings.objectType);
                if (interactObject.Settings.objectType == ObjectSettings.ObjectType.plug)
                {
                    if(plugged != null)
                    {
                        plugged.plug = transform.parent.gameObject;
                    }
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
