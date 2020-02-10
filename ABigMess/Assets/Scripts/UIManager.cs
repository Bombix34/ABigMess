using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField]
    private Image grandmaImage;
    [SerializeField]
    private Image bubbleImage;

    private int currentTextPosition = 0;

    [SerializeField]
    private static float letterApparitionDelay = 0.05f;

    private void Start()
    {
        grandmaImage.color = new Color(1f, 1f, 1f, 0f);
        bubbleImage.color = grandmaImage.color;
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
        transitionInstruction.gameObject.SetActive(true);
        grandmaImage.gameObject.SetActive(true);
        grandmaImage.color = new Color(1f, 1f, 1f, 0f);
        grandmaImage.DOFade(1f, 0.8f);
        bubbleImage.gameObject.SetActive(true);
        bubbleImage.color = grandmaImage.color;
        bubbleImage.DOFade(1f, 0.8f);
        TransitionScreen currentLevelTransition = GameManager.Instance.GetCurrentLevel().introScreen;
        //transitionInstruction.gameObject.SetActive(true);
        if (currentLevelTransition != null)
        {
            backgroundTransitionPanel.color = new Color(currentLevelTransition.backgroundColor.r, currentLevelTransition.backgroundColor.g, currentLevelTransition.backgroundColor.b, 1f);
            transitionInstruction.color = new Color(transitionInstruction.color.r, transitionInstruction.color.g, transitionInstruction.color.b, 1f);
            transitionText.color = backgroundTransitionPanel.color;
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
            if(nextLevelTransition!=null)
            {
                backgroundTransitionPanel.color = new Color(nextLevelTransition.backgroundColor.r, nextLevelTransition.backgroundColor.g, nextLevelTransition.backgroundColor.b, 0f);
            }
        }
        else
        {
            backgroundTransitionPanel.color = new Color(0f, 0f, 0f, 0f);
        }
        transitionInstruction.gameObject.SetActive(false);
        transitionText.text = "";
        transitionPanel.SetActive(true);
        grandmaImage.gameObject.SetActive(false);
        bubbleImage.gameObject.SetActive(false);
        StartCoroutine(TransitionFade(true));
    }

    IEnumerator TransitionFade(bool isFadeIn)
    {
        if(isFadeIn)
        {
            backgroundTransitionPanel.color = new Color(backgroundTransitionPanel.color.r, backgroundTransitionPanel.color.g, backgroundTransitionPanel.color.b, 1);
            transitionText.color = new Color(1f, 1f, 1f, 1f);
            RectTransform transitionPanelRectTransform = transitionPanel.GetComponent<RectTransform>();
            transitionPanelRectTransform.DOAnchorPosY(0, 0.6f).SetEase(Ease.Linear);
            bubbleImage.GetComponent<Image>().DOFade(0f, 1f).Complete();
            bubbleImage.GetComponent<Image>().DOFade(1f, 1f);
        }
        else
        {
            TransitionScreen currentLevelTransition = GameManager.Instance.GetCurrentLevel().introScreen;
            if(currentLevelTransition!=null)
            {
                while (currentTextPosition < currentLevelTransition.textDescription.Length)
                {
                    transitionText.text += currentLevelTransition.textDescription[currentTextPosition];
                    currentTextPosition++;
                    if(GameManager.Instance.PlayersPressValidateInput())
                    {
                        transitionText.text = currentLevelTransition.textDescription;
                        currentTextPosition = currentLevelTransition.textDescription.Length;
                    }
                    yield return new WaitForSeconds(letterApparitionDelay);
                }
                yield return new WaitForSeconds(0.2f);
            }
            while (!GameManager.Instance.PlayersPressValidateInput())
            {
                yield return new WaitForSeconds(0.01f);
            }

            MusicManager.Instance.TransitionLevel(false);

            RectTransform transitionPanelRectTransform = transitionPanel.GetComponent<RectTransform>();
            transitionPanelRectTransform.DOAnchorPosY(transitionPanelRectTransform.rect.height, 0.6f).SetEase(Ease.Linear);
            transitionInstruction.gameObject.SetActive(false);
            GameManager.instance.LaunchLevel();
            
        }
    }

    

}
