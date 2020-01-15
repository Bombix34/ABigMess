using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectTasksManager : MonoBehaviour
{

    public ObjectTaskGroup objectTasksGroup;

    private Canvas canvas;
    private CanvasGroup tasksPanel;
    public GameObject taskPrefab;

    public Dictionary<int, TextMeshProUGUI> taskTexts;
    public Dictionary<int, GameObject> tasksDisplays;
    public Dictionary<int, ObjectTask> tasksToDo;

    public float lastScreenWidth;
    public float lastScreenHeight;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tasksPanel = canvas.transform.Find("Tasks").GetComponent<CanvasGroup>();
        taskTexts = new Dictionary<int, TextMeshProUGUI>();
        tasksDisplays = new Dictionary<int, GameObject>();
        tasksToDo = new Dictionary<int, ObjectTask>();

        AddTasks();
    }

    void Update()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            SetupTasksPositions();
        }

    }

    public void SetupTasksPositions()
    {
        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < tasksDisplays.Count; i++)
        {
            RectTransform taskInstanceRect = tasksDisplays[i].GetComponent<RectTransform>();
            taskInstanceRect.anchoredPosition =
                new Vector2(-width / 2 + taskInstanceRect.sizeDelta.x / 2
                , -height / 2 + (taskInstanceRect.sizeDelta.y / 2) + (taskInstanceRect.sizeDelta.y * i));
        }
    }

    private void TaskDone(int taskId)
    {
        taskTexts[taskId].text = "OK! " + objectTasksGroup.objectTasks[taskId].description;
        taskTexts[taskId].color = Color.yellow;
        tasksToDo.Remove(taskId);
    }

    private void TaskUnDone(int taskId)
    {
        taskTexts[taskId].text = taskTexts[taskId].text.Remove(0, 4);
        taskTexts[taskId].color = Color.white;
        tasksToDo.Add(taskId, objectTasksGroup.objectTasks[taskId]);
    }

    public void OnCollisionEnterTask(GameObject platform)
    {
        TaskDone(platform, true);

        if (tasksToDo.Count == 0)
        {
            objectTasksGroup = objectTasksGroup.nextTasks;
            AddTasks();
        }
    }

    public void OnCollisionExitTask(GameObject platform)
    {
        TaskDone(platform, false);
    }

    private void TaskDone(GameObject platform, bool done)
    {
        // Get the collision that collided with the platform
        Collision collision = platform.gameObject.GetComponent<UnPlugEvent>().collision;

        // Get the object that collided with the platform        
        InteractObject interactObject = collision.gameObject.GetComponent<InteractObject>();

        // No task should work without an interactObject
        if (interactObject == null)
        {
            Debug.LogError("You should use an interact object to do a task");
            return;
        }

        ObjectSettings.ObjectType collisionObjectType = interactObject.Settings.objectType;

        for (int i = 0; i < objectTasksGroup.objectTasks.Count; i++)
        {
            ObjectTask actualTask = objectTasksGroup.objectTasks[i];

            //Debug.Log(collisionObjectType + " != " + actualTask.objectType);
            if (collisionObjectType != actualTask.objectType)
            {
                continue;
            }

            // If the object has no state, then no need to compare a state
            if (collision.gameObject.GetComponent<ObjectState>() == null)
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
            if (VerifyStates(actualTask, collision.gameObject.GetComponent<ObjectState>().states))
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

    private void AddTasks()
    {
        taskTexts.Clear();
        tasksDisplays.Clear();
        tasksToDo.Clear();

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < objectTasksGroup.objectTasks.Count; i++)
        {
            GameObject taskInstance = Instantiate(taskPrefab, tasksPanel.transform);
            RectTransform taskInstanceRect = taskInstance.GetComponent<RectTransform>();
            taskInstanceRect.anchoredPosition =
                new Vector2(-width / 2 + taskInstanceRect.sizeDelta.x / 2
                , -height / 2 - (taskInstanceRect.sizeDelta.y / 2));



            taskInstanceRect.DOAnchorPosY(-height / 2 + (taskInstanceRect.sizeDelta.y / 2) + (taskInstanceRect.sizeDelta.y * i), 1);
            TextMeshProUGUI taskText = taskInstance.GetComponentInChildren<TextMeshProUGUI>();
            taskText.text = objectTasksGroup.objectTasks[i].description;
            taskTexts.Add(i, taskText);
            tasksDisplays.Add(i, taskInstance);
            tasksToDo.Add(i, objectTasksGroup.objectTasks[i]);
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
