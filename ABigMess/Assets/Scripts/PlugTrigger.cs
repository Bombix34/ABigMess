using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlugTrigger : MonoBehaviour
{
    public GameObject pluggedObject = null;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            if(pluggedObject!=null)
            {
                return;
            }
            InteractObject otherIO = other.gameObject.GetComponent<InteractObject>();
            //print(this.transform.parent.gameObject + " is the plug for " + other.name + " that needs to be plugged ?: " + otherIO.Settings.NeedsToBePlugged());
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();
            Plugged plugged = other.gameObject.GetComponent<Plugged>();

            gameObject.GetComponent<InteractObject>().Interact(other.gameObject);

            MusicManager.Instance.GetSoundManager().PlugSFX(true);
            if (otherIO.Settings.NeedsToBePlugged())
            {
                if (objectState != null)
                {
                    objectState.Plugged = true;
                }
                if (plugged != null)
                {
                    plugged.plug = transform.parent.gameObject;
                }
                pluggedObject = other.gameObject;
                GameManager.Instance.TasksManager.UpdateTasksState();
            }
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            if (pluggedObject != null)
            {
                return;
            }
            InteractObject otherIO = other.gameObject.GetComponent<InteractObject>();
            //print(this.transform.parent.gameObject + " is the plug for " + other.name + " that needs to be plugged ?: " + otherIO.Settings.NeedsToBePlugged());
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();
            Plugged plugged = other.gameObject.GetComponent<Plugged>();
            
            if (otherIO.Settings.NeedsToBePlugged())
            {
                if (objectState != null)
                {
                    objectState.Plugged = true;
                }
                if (plugged != null)
                {
                    plugged.plug = transform.parent.gameObject;
                }
                pluggedObject = other.gameObject;
                GameManager.Instance.TasksManager.UpdateTasksState();
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabObject") && other.GetComponent<InteractObject>() != null)
        {
            if(other.gameObject!=pluggedObject)
            {
                return;
            }
            ObjectState objectState = other.gameObject.GetComponent<ObjectState>();

            if (objectState != null)
            {
                objectState.Plugged = false;
            }
            MusicManager.Instance.GetSoundManager().PlugSFX(false);
            pluggedObject = null;
            GameManager.Instance.TasksManager.UpdateTasksState();
        }
    }
}
