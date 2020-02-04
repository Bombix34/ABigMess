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
    private Image transitionInstruction;

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
                taskUI[i].Appear();
                taskUI[i].DisplayTask(newTasks[i], taskIcons.GetIcons(newTasks[i]));
                taskUI[i].UpdateTaskColor();
            }
        }
    }


    public void DisappearTasksUI()
    {
        for (int i = 0; i < taskUI.Count; i++)
        {
            taskUI[i].Disapear();
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
    /// Beggining level transition
    /// </summary>
    public void IntroScreenTransition()
    {
        transitionPanel.SetActive(true);
        TransitionScreen currentLevelTransition = GameManager.Instance.GetCurrentLevel().introScreen;
        //transitionInstruction.gameObject.SetActive(true);
        if (currentLevelTransition != null)
        {
            backgroundTransitionPanel.color = new Color(currentLevelTransition.backgroundColor.r, currentLevelTransition.backgroundColor.g, currentLevelTransition.backgroundColor.b, 1f);
            transitionText.text = currentLevelTransition.textDescription;
            StartCoroutine(TransitionFade(false));
        }
        else
        {
            backgroundTransitionPanel.color = Color.black;
            transitionText.text = "";
            StartCoroutine(TransitionFade(false));
        }
    }

    /// <summary>
    /// End level transition
    /// </summary>
    public void EndLevelTransition()
    {
        if (GameManager.instance.Levels.GetNextLevel() != null)
        {
            TransitionScreen nextLevelTransition = GameManager.instance.Levels.GetNextLevel().introScreen;
            backgroundTransitionPanel.color = new Color(nextLevelTransition.backgroundColor.r, nextLevelTransition.backgroundColor.g, nextLevelTransition.backgroundColor.b, 0f);
        }
        else
        {
            backgroundTransitionPanel.color = new Color(0f, 0f, 0f, 0f);
        }
        transitionInstruction.gameObject.SetActive(false);
        transitionText.text = "";
        transitionPanel.SetActive(true);
        StartCoroutine(TransitionFade(true));
    }

    IEnumerator TransitionFade(bool isFadeIn)
    {
        if(isFadeIn)
        {
            float alphaAmount = 0f;
            while(backgroundTransitionPanel.color.a<1f)
            {
                backgroundTransitionPanel.color = new Color(backgroundTransitionPanel.color.r, backgroundTransitionPanel.color.g, backgroundTransitionPanel.color.b,alphaAmount);
                transitionText.color = new Color(1f, 1f, 1f, alphaAmount);
                alphaAmount += Time.fixedDeltaTime*2f;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            while (!GameManager.Instance.PlayersPressValidateInput())
            {
                yield return new WaitForSeconds(0.01f);
            }
            GameManager.instance.LaunchLevel();
            float alphaAmount = 1f;
            while (backgroundTransitionPanel.color.a > 0.001f)
            {
                backgroundTransitionPanel.color = new Color(0f,0f,0f, alphaAmount);
                transitionInstruction.color = new Color(transitionInstruction.color.r, transitionInstruction.color.g, transitionInstruction.color.b, alphaAmount * 2f);
                transitionText.color = new Color(transitionText.color.r, transitionText.color.g, transitionText.color.b, alphaAmount*2f);
                alphaAmount -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(0.01f);
            }
        }
    }

    

}
