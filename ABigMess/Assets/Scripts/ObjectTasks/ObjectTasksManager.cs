using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

public class ObjectTasksManager : MonoBehaviour
{
    [SerializeField]
    ObjectTaskGroup currentTasks;

    [SerializeField]
    private float delay = 0.5f;

    GameManager manager;

    private SceneObjectDatas objectsDatas;

    private void Awake()
    {
        manager = GetComponent<GameManager>();
        objectsDatas = this.GetComponent<SceneObjectDatas>();
        InitTaskDoneValue();
    }

    private void Start()
    {
        UIManager.Instance.InitTasksUI(currentTasks.objectTasks);
        UpdateTasksState();
    }

    public void InitTaskDoneValue()
    {
        foreach (ObjectTask task in currentTasks.objectTasks)
        {
            task.IsDone = false;
        }
    }

    public void UpdateTasksState()
    {
        int cptTask = 0;
        foreach(ObjectTask task in currentTasks.objectTasks)
        {
            if(task.IsTaskDone(objectsDatas))
            {
                cptTask++;
            }
        }
        
        if(cptTask>=currentTasks.objectTasks.Count)
        {
            LoadNextTaskGroup();
        }
        UIManager.Instance.UpdateTasksUI();
    }

    private void LoadNextTaskGroup()
    {
        currentTasks = currentTasks.nextTasks;

        if (currentTasks==null)
        {
            GameManager.Instance.WinCurrentLevel();
            return;
        }

        InitTaskDoneValue();
        UIManager.Instance.InitTasksUI(currentTasks.objectTasks);
        UpdateTasksState();
        
    }

    #region OLD_CODE
    /*
    private ObjectTaskGroup actualObjectTasksGroup;
    private int countTasks;
    public ObjectTaskGroup objectTasksGroup;
    private List<ObjectTask> allTasks;

    private Canvas canvas;
    private CanvasGroup tasksPanel;
    public GameObject taskPrefab;

    private Dictionary<int, TaskUI> tasksUI;
    private Dictionary<int, GameObject> taskInstances;
    private Dictionary<int, ObjectTask> tasksToDo;
    private Dictionary<int, List<GameObject>> objectsInTask;

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
        objectsInTask = new Dictionary<int, List<GameObject>>();

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
            objectsInTask.Add(i, new List<GameObject>());
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

        if (sweepAwayTasksTime > 0)
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
                if(actualObjectTasksGroup==null)
                {
                    GameManager.Instance.WinCurrentLevel();
                }
                    
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
        }
    }

    private void TaskDone(GameObject g, int taskId)
    {
        if (!objectsInTask[taskId].Contains(g))
        {
            objectsInTask[taskId].Add(g);
        }

        Debug.Log(objectsInTask[taskId].Count + " -- " + tasksToDo[taskId].count);
        if (objectsInTask[taskId].Count >= allTasks[taskId].count)
        {
            tasksUI[taskId].TaskDone();

            if (tasksToDo.ContainsKey(taskId))
            {
                tasksToDo.Remove(taskId);

                sweepAwayTasksTime = SweepAwayTasksTime;
            }
        }
    }

    private void TaskUnDone(GameObject g, int taskId)
    {
        if (objectsInTask[taskId].Contains(g))
        {
            objectsInTask[taskId].Remove(g);
        }

        //Debug.Log("UNDONE : " + objectsInTask[taskId].Count + " -- " + allTasks[taskId].count);
        // Verify tasks done or undone

        if (objectsInTask[taskId].Count < allTasks[taskId].count)
        {
            tasksUI[taskId].TaskUnDone();
        }

        if (!tasksToDo.ContainsKey(taskId))
        {
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
        Collider collider = null;
        GameObject colliderGameObject = null;
        if (platform.gameObject.GetComponent<OnTriggerEvents>() != null)
        {
            collider = platform.gameObject.GetComponent<OnTriggerEvents>().collider;
        }
        else if (platform.gameObject.GetComponentInChildren<OnTriggerEvents>() != null)
        {
            collider = platform.gameObject.GetComponentInChildren<OnTriggerEvents>().collider;
        }

        if (platform.gameObject.GetComponent<PlayerManager>() != null)
        {
            
        }
        else if (collider == null)
        {
            Debug.LogError("You should use a Trigger event with the apropriate gameObject passed as parameter");
            return;
        }

        // Get the object that collided with the platform        
        InteractObject interactObject = null;
        if (platform.gameObject.GetComponent<PlayerManager>() != null)
        {
            interactObject = platform.GetComponent<PlayerManager>().GrabbedObject.GetComponent<InteractObject>();
            colliderGameObject = platform.GetComponent<PlayerManager>().GrabbedObject;
        } else {
            interactObject = collider.gameObject.GetComponent<InteractObject>();
            colliderGameObject = collider.gameObject;
        }
        Debug.Log("Interact Object name : " + interactObject.name);

        // No task should work without an interactObject
        if (interactObject == null)
        {
            Debug.LogError("You should use an interact object to do a task, object name = " + collider.gameObject.name + " on platform = " + platform.name);
            //Debug.LogError("You should use an interact object to do a task");
            return;
        }

        if (interactObject.Settings == null)
        {
            //Debug.LogError("You should define settings for the object");
            return;
        }

        ObjectSettings.ObjectType collisionObjectType = interactObject.Settings.objectType;

        for (int taskId = countTasks; taskId < allTasks.Count; taskId++)
        {
            ObjectTask actualTask = allTasks[taskId];

            //Debug.Log(collisionObjectType + " != " + actualTask.objectType);
            if (collisionObjectType != actualTask.objectType)
            {
                continue;
            }

            //Debug.Log("Has ObjectState: " + (collider.gameObject.GetComponent<ObjectState>() == null).ToString());
            // If the object has no state, then no need to compare a state
            if (colliderGameObject.GetComponent<ObjectState>() == null)
            {
                // Check if the task destination is the same as the passed plateform 
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
                else if (actualTask.destination.Equals("")) // If no destination has been set, any platform works
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
                continue;
            }

            // If the object has a state, then veryfy if the states of the object correspond with what is wanted
            if (done && VerifyStates(actualTask, colliderGameObject.GetComponent<ObjectState>().states))
            {
                //print(gameObject.name + "  " + actualTask.destination);
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
            }
            else if (!done)
            {
                if (actualTask.destination.Equals(platform.name))
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
                else if (actualTask.destination.Equals(""))
                {
                    TasksDoneOrUndone(colliderGameObject, done, taskId);
                }
            }
        }
        
    }

    private void TasksDoneOrUndone(GameObject g, bool done, int i)
    {
        if (done)
        {
            TaskDone(g, i);
        }
        else
        {
            TaskUnDone(g, i);
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
    */
    #endregion

}


