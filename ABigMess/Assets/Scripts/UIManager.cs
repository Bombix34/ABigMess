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

    [SerializeField]
    GameObject transitionPanel;

    public void UpdateChronoUI(int minutes, int seconds)
    {
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

    public void FadeIn()
    {
        transitionPanel.SetActive(true);
        Animator transitionAnim = transitionPanel.GetComponent<Animator>();
        transitionAnim.SetTrigger("FadeIn");
        transitionAnim.SetInteger("TransitionNb", 1);
    }

    public void FadeOut()
    {
        transitionPanel.SetActive(true);
        Animator transitionAnim = transitionPanel.GetComponent<Animator>();
        transitionAnim.SetTrigger("FadeOut");
        transitionAnim.SetInteger("TransitionNb",1);
    }

}
