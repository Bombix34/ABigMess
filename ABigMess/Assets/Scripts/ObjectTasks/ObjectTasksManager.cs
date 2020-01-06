using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectTasksManager : MonoBehaviour
{

    public List<ObjectTask> objectTasks;
    public ObjectTask actualTask;

    public TextMeshProUGUI actualTaskText;

    void Start()
    {
        GetNextTask();
    }

    void Update()
    {


    }

    public void OnCollisionTask(GameObject gameObject)
    {
        Collision collision = gameObject.GetComponent<CollideEvent>().collision;

        if(collision.gameObject.GetComponent<ObjectState>() == null)
        {
            // If the object has no state, then no need to compare a state
            GetNextTask();
            return;
        }

        ObjectSettings.ObjectType collisionObjectType = collision.gameObject.GetComponent<InteractObject>().Settings.objectType;

        if (collisionObjectType != actualTask.objectType)
        {
            return;
        }

        if (VerifyStates(actualTask, collision.gameObject.GetComponent<ObjectState>().states))
        {
            GetNextTask();
        }
    }

    /// <summary>
    /// Verify if each state needs to be set: if it needs to be set and the value is different
    /// Then return false
    /// else (if we chacked that every state is of the true value) return true
    /// </summary>
    /// <param name="actualTask"></param>
    /// <param name="states"></param>
    /// <returns></returns>
    private bool VerifyStates(ObjectTask actualTask, ObjectStates states)
    {
        if(CheckBoolPairState(actualTask.washed, states.washed))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.burnt, states.burnt))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.smuged, states.smuged))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.cooked, states.cooked))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.grown, states.grown))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.colored, states.colored))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.broken, states.broken))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.opened, states.opened))
        {
            return false;
        }
        if (CheckBoolPairState(actualTask.plugged, states.plugged))
        {
            return false;
        }
        return true;
    }

    bool CheckBoolPairState(BoolPair pair, bool state)
    {
        return pair.Key && pair.Value != state;
    }

    void GetNextTask()
    {
        if (objectTasks.Count > 0)
        {
            actualTask = objectTasks[0];
            actualTaskText.text = actualTask.description;
            objectTasks.RemoveAt(0);
        }
        else
        {
            actualTaskText.text = "All tasks done";
        }

    }





}
