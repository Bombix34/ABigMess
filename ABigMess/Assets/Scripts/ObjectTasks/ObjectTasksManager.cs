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


        float distance = Vector3.Distance(actualTask.gameObject.transform.position, actualTask.desiredPosition.position);
        if (distance < 2.0f)
        {
            if (actualTask.states.Equals(actualTask.gameObject.GetComponent<ObjectState>().states))
            {
                GetNextTask();
            }
        }

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
