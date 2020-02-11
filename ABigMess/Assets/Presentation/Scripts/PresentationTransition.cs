using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class PresentationTransition : MonoBehaviour
{
    [SerializeField]
    private Color transitionScreenColorIn;
    [SerializeField]
    private Color transitionScreenColorOut;
    [SerializeField]
    GameObject transitionPanel;
    [SerializeField]
    private Image backgroundTransitionPanel;

    public UnityEvent OnStartSceneEvent;
    public UnityEvent OnLeaveSceneEvent;

    private void Start()
    {
        if(OnStartSceneEvent==null)
        {
            OnStartSceneEvent = new UnityEvent();
        }
        if(OnLeaveSceneEvent==null)
        {
            OnLeaveSceneEvent = new UnityEvent();
        }
    }

    public void LeaveSceneTransition()
    {
        OnLeaveSceneEvent.Invoke();
    }

    public void StartSceneTransition()
    {
        OnStartSceneEvent.Invoke();
    }


    public void GoOutTransition()
    {
        transitionPanel.SetActive(true);
        backgroundTransitionPanel.color = transitionScreenColorOut;
        RectTransform transitionPanelRectTransform = transitionPanel.GetComponent<RectTransform>();
        transitionPanelRectTransform.DOAnchorPosY(transitionPanelRectTransform.rect.height, 0.6f).SetEase(Ease.Linear);
        //MusicManager.Instance.TransitionLevel(false);
    }

    public void GoInTransition()
    {
        backgroundTransitionPanel.color = transitionScreenColorIn;
        RectTransform transitionPanelRectTransform = transitionPanel.GetComponent<RectTransform>();
        transitionPanelRectTransform.anchoredPosition = new Vector2(0f, transitionPanelRectTransform.rect.height);
        //transitionPanelRectTransform.DOAnchorPosY(transitionPanelRectTransform.rect.height, 0f);
        transitionPanelRectTransform.DOAnchorPosY(0, 0.6f).SetEase(Ease.Linear);
        transitionPanel.SetActive(true);
    }



}
