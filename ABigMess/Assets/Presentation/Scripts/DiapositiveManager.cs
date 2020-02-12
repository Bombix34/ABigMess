using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DiapositiveManager : MonoBehaviour
{
    public List<GameObject> diapositives;
    private int curIndexSlide = 0;

    private PlayerInputManager[] playersInput;

    private void Start()
    {
        playersInput = GetComponents<PlayerInputManager>();
    }

    private void Update()
    {
        UpdateSlideInput();
    }

    public void UpdateSlideInput()
    {
        for(int i =0; i < playersInput.Length;++i)
        {
            if(playersInput[i].GetGrabInputDown())
            {
                SwitchSlide(true);
            }
            else if (playersInput[i].GetInteractInputDown())
            {
                SwitchSlide(false);
            }
        }
    }

    public void SwitchSlide(bool isNext)
    {
        if(isNext)
        {
            if(curIndexSlide>=diapositives.Count-1)
            {
                return;
            }
            NextSlideTransition(diapositives[curIndexSlide]);
            curIndexSlide++;
        }
        else
        {
            if(curIndexSlide==0)
            {
                return;
            }
            curIndexSlide--;
            PreviousSlideTransition(diapositives[curIndexSlide]);
        }
    }

    public void NextSlideTransition(GameObject slide)
    {
        RectTransform transitionPanelRectTransform = slide.GetComponent<RectTransform>();
        transitionPanelRectTransform.DOAnchorPosY(transitionPanelRectTransform.rect.height, 0.6f).SetEase(Ease.Linear);
        //MusicManager.Instance.TransitionLevel(false);
    }

    public void PreviousSlideTransition(GameObject slide)
    {
        RectTransform transitionPanelRectTransform = slide.GetComponent<RectTransform>();
        transitionPanelRectTransform.DOAnchorPosY(0f, 0.6f).SetEase(Ease.Linear);
    }
}
