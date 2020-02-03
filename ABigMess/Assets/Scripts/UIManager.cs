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
    [SerializeField]
    private Image backgroundTransitionPanel;
    [SerializeField]
    private Text transitionText;
    [SerializeField]
    private Text transitionInstruction;

    private void Start()
    {
        transitionInstruction.gameObject.SetActive(false);
    }

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
        //clear all tasks
        for (int i = 0; i < taskUI.Count; i++)
        {
            taskUI[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < newTasks.Count;++i)
        {
            if (newTasks[i] != null)
            {
                taskUI[i].gameObject.SetActive(true);
                taskUI[i].DisplayTask(newTasks[i], taskIcons.GetIcons(newTasks[i]));
                taskUI[i].UpdateTaskColor();
            }
        }
    }

    public void UpdateTasksUI()
    {
        for(int i = 0; i < taskUI.Count;i++)
        {
            taskUI[i].UpdateTaskColor();
            taskUI[i].UpdateNumber();
        }
    }

    /// <summary>
    /// End level transition
    /// </summary>
    public void FadeInTransition()
    {
        transitionPanel.SetActive(true);
        TransitionScreen currentLevelTransition = GameManager.Instance.GetCurrentLevel().endScreen;
        transitionInstruction.gameObject.SetActive(false);
        if (currentLevelTransition != null)
        {
            backgroundTransitionPanel.color = new Color(currentLevelTransition.backgroundColor.r, currentLevelTransition.backgroundColor.g, currentLevelTransition.backgroundColor.b, 0f);
            transitionText.text = currentLevelTransition.textDescription;
            StartCoroutine(TransitionFade(currentLevelTransition,true));
        }
        else
        {
            backgroundTransitionPanel.color = Color.black;
            transitionText.text = "";
        }
    }

    IEnumerator TransitionFade(TransitionScreen transitionProperty, bool isFadeIn)
    {
        if(isFadeIn)
        {
            float alphaAmount = 0f;
            while(backgroundTransitionPanel.color.a<1f)
            {
                backgroundTransitionPanel.color = new Color(transitionProperty.backgroundColor.r, transitionProperty.backgroundColor.g, transitionProperty.backgroundColor.b,alphaAmount);
                transitionText.color = new Color(1f, 1f, 1f, alphaAmount);
                alphaAmount += Time.fixedDeltaTime*2f;
                if(GameManager.Instance.PlayersPressValidateInput())
                {
                    backgroundTransitionPanel.color = transitionProperty.backgroundColor;
                    transitionText.color = Color.white;
                }
                yield return new WaitForSeconds(0.01f);
            }
            transitionInstruction.gameObject.SetActive(true);
        }
        else
        {
            float alphaAmount = 1f;
            transitionText.text = "";
            while (backgroundTransitionPanel.color.a > 0.001f)
            {
                backgroundTransitionPanel.color = new Color(0f,0f,0f, alphaAmount);
                alphaAmount -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    /// <summary>
    /// Begin level transition
    /// </summary>
    public void FadeOutTransition()
    {
        transitionPanel.SetActive(true);
        StartCoroutine(TransitionFade(null, false));
    }

}
