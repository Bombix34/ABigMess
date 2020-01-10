using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectTasksManager : MonoBehaviour
{

    public List<ObjectTask> objectTasks;
    private ObjectTask actualTask;

    private Canvas canvas;
    private GridLayoutGroup tasksPanel;
    public GameObject taskPrefab;

    public Dictionary<int, TextMeshProUGUI> taskTexts;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tasksPanel = canvas.transform.Find("Tasks").GetComponent<GridLayoutGroup>();
        taskTexts = new Dictionary<int, TextMeshProUGUI>();

        for (int i = 0; i < objectTasks.Count; i++)
        {
            GameObject taskInstance = Instantiate(taskPrefab, tasksPanel.transform);
            TextMeshProUGUI taskText = taskInstance.GetComponentInChildren<TextMeshProUGUI>();
            taskText.text = objectTasks[i].description;
            taskTexts.Add(i, taskText);
        }
    }

    void Update()
    {


    }

    private void TaskDone(int taskId)
    {
         taskTexts[taskId].text = "OK! " + objectTasks[taskId].description;
         taskTexts[taskId].color = Color.yellow;

    }

    public void OnCollisionTask(GameObject gameObject)
    {
        Collision collision = gameObject.GetComponent<CollideEvent>().collision;

        if(collision.gameObject.GetComponent<InteractObject>() == null)
        {
            return;
        }

        ObjectSettings.ObjectType collisionObjectType = collision.gameObject.GetComponent<InteractObject>().Settings.objectType;

        for (int i = 0; i < objectTasks.Count; i++)
        {
            actualTask = objectTasks[i];

            if (collisionObjectType != actualTask.objectType)
            {
                continue;
            }

            if (collision.gameObject.GetComponent<ObjectState>() == null)
            {
                // If the object has no state, then no need to compare a state
                if (actualTask.destination.Equals(gameObject.name))
                {
                    TaskDone(i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TaskDone(i);
                }
                continue;
            }

            if (VerifyStates(actualTask, collision.gameObject.GetComponent<ObjectState>().states))
            {
                //print(gameObject.name + "  " + actualTask.destination);
                if (actualTask.destination.Equals(gameObject.name))
                {
                    TaskDone(i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TaskDone(i);
                }
            }
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


}
