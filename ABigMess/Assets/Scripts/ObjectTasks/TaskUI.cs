using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    [SerializeField]
    private Text textField;

    ObjectTask task;

    void Awake()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void DisplayTask(ObjectTask newTask)
    {
        if(newTask==null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        task = newTask;
        textField.text = newTask.taskName;
        print("wow");
        this.gameObject.SetActive(true);
    }

    public void UpdateTaskColor()
    {
        if (task == null)
        {
            return;
        }
        GetComponent<Image>().color = task.IsDone ? Color.yellow : Color.white;
    }

    public void SetText(string description)
    {
    }

    public void SetObjectTask(ObjectTask objectTask)
    {
        
    }

    #region GET/SET


    #endregion
}
