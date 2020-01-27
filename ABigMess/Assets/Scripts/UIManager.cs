using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Text chronoField;

    [SerializeField]
    List<TaskUI> taskUI;

    List<ObjectTask> currentTasks;

    [SerializeField]
    TaskIcons taskIcons;

    public void UpdateChronoUI(float currentTime)
    {
        if(currentTime<0)
        {
            currentTime = 0f;
        }
        int minutes = (int)(currentTime / 60f);
        int seconds = (int)(currentTime % 60f);
        if(seconds < 10)
        {
            chronoField.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            chronoField.text ="0"+minutes.ToString()+":"+seconds.ToString();  
        }
    }

    public void InitTasksUI(List<ObjectTask> newTasks)
    {
        currentTasks = newTasks;
        for(int i = 0; i < newTasks.Count;++i)
        {
            taskUI[i].DisplayTask(newTasks[i], taskIcons.GetIcons(newTasks[i]));
            taskUI[i].UpdateTaskColor();
        }
    }

    public void UpdateTasksUI()
    {
        for(int i = 0; i < taskUI.Count;i++)
        {
            taskUI[i].UpdateTaskColor();
        }
    }

}
