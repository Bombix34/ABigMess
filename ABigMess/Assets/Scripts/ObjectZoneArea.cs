using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectZoneArea : MonoBehaviour
{
    private List<InteractObject> objectsInZone;

    private void Awake()
    {
        objectsInZone = new List<InteractObject>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractObject>() != null)
        {
            if (!objectsInZone.Contains(other.GetComponent<InteractObject>()))
            {
                objectsInZone.Add(other.GetComponent<InteractObject>());
                GameManager.Instance.TasksManager.UpdateTasksState();
            }
        }
    }
    /*
    public void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InteractObject>() != null)
        {
            if (!objectsInZone.Contains(other.GetComponent<InteractObject>()))
            {
                objectsInZone.Add(other.GetComponent<InteractObject>());
                GameManager.Instance.TasksManager.UpdateTasksState();
            }
        }
    }
    */
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractObject>() != null)
        {
            objectsInZone.Remove(other.GetComponent<InteractObject>());
            GameManager.Instance.TasksManager.UpdateTasksState();
        }
    }

    #region GET/SET

    public List<InteractObject> GetObjectsInZone()
    {
        return objectsInZone;
    }

    #endregion


}
