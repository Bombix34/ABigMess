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

    [SerializeField]
    private Text numberField;

    [SerializeField]
    private GameObject objectIcon;

    [SerializeField]
    private GameObject actionIcon;

    [SerializeField]
    private GameObject destinationIcon;

    ObjectTask task;

    void Awake()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void DisplayTask(ObjectTask newTask, List<Sprite> list)
    {
        if(newTask==null)
        {
            this.gameObject.SetActive(false);
            return;
        }
        task = newTask;
        if (list.Count == 0)
        {
            textField.text = newTask.taskName;
        } else
        {
            textField.gameObject.SetActive(false);

            numberField.text = newTask.GetCountForInterface().ToString();

            if (list.Count >= 1)
            {
                objectIcon.SetActive(list[0] != null ? true : false);
                objectIcon.GetComponent<Image>().sprite = list[0] != null ? list[0] : objectIcon.GetComponent<Image>().sprite;
            }
            if (list.Count >= 2)
            {
                actionIcon.SetActive(list[1] != null ? true : false);
                actionIcon.GetComponent<Image>().sprite = list[1] != null ? list[1] : actionIcon.GetComponent<Image>().sprite;
            }
            if (task.eventType==ObjectTask.EventKeyWord.bring)
            {
                destinationIcon.SetActive(true);
                destinationIcon.GetComponent<Image>().sprite = task.destinationSprite;
            }
            else
            {
                destinationIcon.SetActive(false);
            }
        }
        this.gameObject.SetActive(true);
    }

    public void UpdateNumber()
    {
        if(task==null)
        {
            return;
        }
        numberField.text = task.GetCountForInterface().ToString();
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
