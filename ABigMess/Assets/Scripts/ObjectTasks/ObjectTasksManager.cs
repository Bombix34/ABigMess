using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectTasksManager : MonoBehaviour
{

    public List<ObjectTask> objectTasks;
    private ObjectTask actualTask;

    private Canvas canvas;
    private CanvasGroup tasksPanel;
    public GameObject taskPrefab;

    public Dictionary<int, TextMeshProUGUI> taskTexts;
    public Dictionary<int, GameObject> tasksDisplays;

    public float lastScreenWidth;
    public float lastScreenHeight;

    void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        tasksPanel = canvas.transform.Find("Tasks").GetComponent<CanvasGroup>();
        taskTexts = new Dictionary<int, TextMeshProUGUI>();
        tasksDisplays = new Dictionary<int, GameObject>();

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float height = tasksPanel.GetComponent<RectTransform>().rect.height;
        float width = tasksPanel.GetComponent<RectTransform>().rect.width;

        for (int i = 0; i < objectTasks.Count; i++)
        {
            GameObject taskInstance = Instantiate(taskPrefab, tasksPanel.transform);
            RectTransform taskInstanceRect = taskInstance.GetComponent<RectTransform>();
            taskInstanceRect.anchoredPosition = 
                new Vector2(-width / 2 + taskInstanceRect.sizeDelta.x/2
                , -height / 2 + (taskInstanceRect.sizeDelta.y/ 2) + (taskInstanceRect.sizeDelta.y * i));


            //taskInstanceRect.DOAnchorPosX(-786, 1);
            //taskInstanceRect.DOAnchorPosY((190 * (objectTasks.Count -i)) - 400, 1);
            TextMeshProUGUI taskText = taskInstance.GetComponentInChildren<TextMeshProUGUI>();
            taskText.text = objectTasks[i].description;
            taskTexts.Add(i, taskText);
            tasksDisplays.Add(i, taskInstance);
        }
    }

    void Update()
    {
        if(lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
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
         taskTexts[taskId].text = "OK! " + objectTasks[taskId].description;
         taskTexts[taskId].color = Color.yellow;

    }

    private void TaskUnDone(int taskId)
    {
        taskTexts[taskId].text = taskTexts[taskId].text.Remove(0, 4);
        taskTexts[taskId].color = Color.white;

    }

    public void OnCollisionEnterTask(GameObject gameObject)
    {
        Collision collision = gameObject.GetComponent<UnPlugEvent>().collision;

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

    public void OnCollisionExitTask(GameObject gameObject)
    {
        Collision collision = gameObject.GetComponent<UnPlugEvent>().collision;

        if (collision.gameObject.GetComponent<InteractObject>() == null)
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
                    TaskUnDone(i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TaskUnDone(i);
                }
                continue;
            }

            if (VerifyStates(actualTask, collision.gameObject.GetComponent<ObjectState>().states))
            {
                //print(gameObject.name + "  " + actualTask.destination);
                if (actualTask.destination.Equals(gameObject.name))
                {
                    TaskUnDone(i);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TaskUnDone(i);
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
