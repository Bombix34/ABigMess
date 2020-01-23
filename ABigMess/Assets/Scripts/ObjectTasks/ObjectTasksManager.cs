﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ObjectTasksManager : MonoBehaviour
{

    private ObjectTaskGroup actualObjectTasksGroup;
    private int countTasks;
    public ObjectTaskGroup objectTasksGroup;
    public List<ObjectTask> allTasks;

    private Canvas canvas;
    private CanvasGroup tasksPanel;
    public GameObject taskPrefab;

    public Dictionary<int, TaskUI> tasksUI;
    public Dictionary<int, GameObject> taskInstances;
    public Dictionary<int, ObjectTask> tasksToDo;

    private float lastScreenWidth;
    private float lastScreenHeight;

    [Range(0, 100)]
    public float marginX = 25;
    [Range(0, 100)]
    public float marginY = 25;

    public float SweepAwayTasksTime = 2f;
    private float sweepAwayTasksTime;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tasksPanel = canvas.transform.Find("Tasks").GetComponent<CanvasGroup>();
        tasksUI = new Dictionary<int, TaskUI>();
        taskInstances = new Dictionary<int, GameObject>();
        tasksToDo = new Dictionary<int, ObjectTask>();
        allTasks = new List<ObjectTask>();

        actualObjectTasksGroup = objectTasksGroup;

        while (actualObjectTasksGroup != null)
        {
            allTasks.AddRange(actualObjectTasksGroup.objectTasks);
            if (actualObjectTasksGroup == actualObjectTasksGroup.nextTasks)
            {
                actualObjectTasksGroup = null;
                continue;
            }
            actualObjectTasksGroup = actualObjectTasksGroup.nextTasks;
        }

        actualObjectTasksGroup = objectTasksGroup;

        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        // add all tasks to TasksToDo, but dont add the displays
        for (int i = 0; i < allTasks.Count; i++)
        {
            GameObject taskInstance = Instantiate(taskPrefab, tasksPanel.transform);
            RectTransform taskInstanceRect = taskInstance.GetComponent<RectTransform>();
            TaskUI taskUI = taskInstance.GetComponent<TaskUI>();
            taskInstanceRect.anchoredPosition =
                new Vector2(-width / 2 - taskInstanceRect.sizeDelta.x / 2
                , height / 2 + (taskInstanceRect.sizeDelta.y / 2));
            taskUI.SetObjectTask(allTasks[i]);
            tasksUI.Add(i, taskUI);
            taskInstances.Add(i, taskInstance);
            tasksToDo.Add(i, allTasks[i]);
        }

        AddTasks();
    }

    private void AddTasks()
    {

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < actualObjectTasksGroup.objectTasks.Count; i++)
        {
            GameObject taskInstance = taskInstances[i + countTasks];
            RectTransform taskInstanceRect = taskInstance.GetComponent<RectTransform>();
            taskInstanceRect.anchoredPosition =
                new Vector2(-width / 2 + taskInstanceRect.sizeDelta.x / 2 + marginX
                , height / 2 + (taskInstanceRect.sizeDelta.y / 2) + marginY);


            // Make the tasks appear from the top
            taskInstanceRect.DOAnchorPosY(height / 2 - (taskInstanceRect.sizeDelta.y / 2) - (taskInstanceRect.sizeDelta.y * i) - marginY, 1);
        }
    }

    void Update()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            SetupTasksPositions();
        }

        if(sweepAwayTasksTime > 0)
        {
            sweepAwayTasksTime -= Time.deltaTime;
        }

        if (actualObjectTasksGroup != null && sweepAwayTasksTime <= 0)
        {
            if (ActualTasksDone())
            {
                ClearTasksGroup();
                sweepAwayTasksTime = SweepAwayTasksTime;
                countTasks += actualObjectTasksGroup.objectTasks.Count;

                // When tasks from a group are done
                if (actualObjectTasksGroup == actualObjectTasksGroup.nextTasks)
                {
                    actualObjectTasksGroup = null;
                    return;
                }

                actualObjectTasksGroup = actualObjectTasksGroup.nextTasks;

                AddTasks();
            }
        }

    }

    public void SetupTasksPositions()
    {
        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        /*for (int i = 0; i < taskInstances.Count; i++)
        {
            RectTransform taskInstanceRect = taskInstances[i].GetComponent<RectTransform>();
            taskInstanceRect.anchoredPosition =
                new Vector2(-width / 2 + taskInstanceRect.sizeDelta.x / 2 + marginX
                , height / 2 - (taskInstanceRect.sizeDelta.y / 2) - (taskInstanceRect.sizeDelta.y * i) - marginY);
        }*/
    }

    private void TaskDone(int taskId)
    {
        if (tasksToDo.ContainsKey(taskId))
        {
            tasksUI[taskId].TaskDone();

            tasksToDo.Remove(taskId);

            sweepAwayTasksTime = SweepAwayTasksTime;
        }
    }

    private void TaskUnDone(int taskId)
    {
        if (!tasksToDo.ContainsKey(taskId))
        {
            tasksUI[taskId].TaskUnDone();

            tasksToDo.Add(taskId, allTasks[taskId]);

            sweepAwayTasksTime = SweepAwayTasksTime;
        }
    }

    public void OnCollisionEnterTask(GameObject platform)
    {
        TaskDone(platform, true);
    }

    public bool ActualTasksDone()
    {
        bool done = true;
        for (int i = 0; i < actualObjectTasksGroup.objectTasks.Count; i++)
        {
            if (tasksToDo.ContainsValue(actualObjectTasksGroup.objectTasks[i]))
            {
                done = false;
            }
        }

        return done;
    }

    public void OnCollisionExitTask(GameObject platform)
    {
        //Debug.Log("Collision Exit Task from " + platform + " on " + platform.gameObject.GetComponent<OnTriggerEvents>().collider.name);
        TaskDone(platform, false);
    }

    private void TaskDone(GameObject platform, bool done)
    {
        // Get the collision that collided with the platform
        Collider collider;
        if (platform.gameObject.GetComponent<OnTriggerEvents>() != null)
        {
            collider = platform.gameObject.GetComponent<OnTriggerEvents>().collider;
        } else
        {
            collider = platform.gameObject.GetComponentInChildren<OnTriggerEvents>().collider;
        }

        if(collider == null)
        {
            Debug.LogError("You should use a Trigger event with the apropriate gameObject passed as parameter");
            return;
        }

        // Get the object that collided with the platform        
        InteractObject interactObject = collider.gameObject.GetComponent<InteractObject>();
        //Debug.Log("Interact Object name : " + collider.gameObject.GetComponent<InteractObject>());

        // No task should work without an interactObject
        if (interactObject == null)
        {
            //Debug.LogError("You should use an interact object to do a task, object name = " + collider.gameObject.name + " on platform = " + platform.name);
            //Debug.LogError("You should use an interact object to do a task");
            return;
        }

        if(interactObject.Settings == null)
        {
            //Debug.LogError("You should define settings for the object");
            return;
        }
        else if(interactObject.Settings==null)
        {
            Debug.LogError("Missing object settings");
            return;
        }
        ObjectSettings.ObjectType collisionObjectType = interactObject.Settings.objectType;

        for (int i = countTasks; i < allTasks.Count; i++)
        {
            ObjectTask actualTask = allTasks[i];

            //Debug.Log(collisionObjectType + " != " + actualTask.objectType);
            if (collisionObjectType != actualTask.objectType)
            {
                continue;
            }

            //Debug.Log("Has ObjectState: " + (collider.gameObject.GetComponent<ObjectState>() == null).ToString());
            // If the object has no state, then no need to compare a state
            if (collider.gameObject.GetComponent<ObjectState>() == null)
            {
                // Check if the task destination is the same as the passed plateform 
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(done, i);
                }
                else if (actualTask.destination.Equals("")) // If no destination has been set, any platform works
                {
                    TasksDoneOrUndone(done, i);
                }
                continue;
            }

            // If the object has a state, then veryfy if the states of the object correspond with what is wanted
            if (done && VerifyStates(actualTask, collider.gameObject.GetComponent<ObjectState>().states))
            {
                //print(gameObject.name + "  " + actualTask.destination);
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(done, i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TasksDoneOrUndone(done, i);
                }
            }
            else if (!done)
            {
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(done, i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TasksDoneOrUndone(done, i);
                }
            }
        }
    }

    private void TasksDoneOrUndone(bool done, int i)
    {
        if (done)
        {
            TaskDone(i);
        }
        else
        {
            TaskUnDone(i);
        }
    }



    private void ClearTasksGroup()
    {
        lastScreenWidth = Screen.width;

        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < actualObjectTasksGroup.objectTasks.Count; i++)
        {
            RectTransform taskDisplay = taskInstances[i + countTasks].GetComponent<RectTransform>();
            taskDisplay.DOAnchorPosX(-width / 2 - taskDisplay.sizeDelta.x / 2, 0.5f);
            Destroy(taskDisplay.gameObject, 0.5f);

            tasksUI.Remove(i + countTasks);
            taskInstances.Remove(i + countTasks);
            tasksToDo.Remove(i + countTasks);
            //allTasks.Remove(allTasks[i + countTasks]);
        }


    }

    /// <summary>
    /// Verify if each state needs to be set: if it needs to be set and the value is different
    /// Then return false
    /// else (if we checked that every state is of the true value) return true
    /// </summary>
    /// <param name="actualTask"></param>
    /// <param name="states"></param>
    /// <returns></returns>
    private bool VerifyStates(ObjectTask actualTask, ObjectStates states)
    {
        if (CheckBoolPairState(actualTask.washed, states.washed))
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
