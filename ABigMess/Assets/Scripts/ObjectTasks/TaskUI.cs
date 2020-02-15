using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    RectTransform rectTransform;
    Image image;

    private Vector2 initialSizeDelta;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        rectTransform.anchoredPosition.Set(rectTransform.anchoredPosition.x, rectTransform.sizeDelta.y);
        image.color = Color.white;

        initialSizeDelta = rectTransform.sizeDelta;
    }

    public void DisplayTask(ObjectTask newTask, List<Sprite> list)
    {
        task = newTask;
        if (list.Count == 0)
        {
            textField.text = newTask.taskName;
        }
        else
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
            if (task.eventType == ObjectTask.EventKeyWord.bring)
            {
                destinationIcon.SetActive(true);
                destinationIcon.GetComponent<Image>().sprite = task.destinationSprite;
            }
            else
            {
                destinationIcon.SetActive(false);
            }
        }
    }

    public void Disapear()
    {
        if ((task != null && task.IsDone))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(rectTransform.DOAnchorPosY(rectTransform.sizeDelta.y, 1));
        }
    }

    public void Appear()
    {
        if ((task == null || !task.IsDone))
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(image.DOColor(Color.white, 1));
            sequence.Join(rectTransform.DOAnchorPosY(-rectTransform.sizeDelta.y / 2, 1));
        }
    }

    public void UpdateNumber()
    {
        if (task == null)
        {
            return;
        }
        if (task.showCounterUI)
        {
            numberField.text = task.GetCountForInterface().ToString();
        }
        else
        {
            numberField.text = "";
        }
    }

    public void UpdateTaskColor()
    {
        if (task == null)
        {
            return;
        }
        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(task.IsDone ? new Color(0f,1f,0f,0.7f) : new Color(1f,1f,1f,0.7f), 1));
        sequence.Join(rectTransform.DOSizeDelta(task.IsDone ? initialSizeDelta * 1.125f : initialSizeDelta, 1f).SetEase(Ease.InElastic));
        
    }

    #region GET/SET


    #endregion
}
