using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    TextMeshProUGUI text;
    public GameObject imagePrefab;
    private RectTransform rectTransform;

    [Range(0, 50)]
    public float marginX = 10;
    [Range(0, 50)]
    public float marginY = 10;

    [Range(0, 240)]
    public float size = 110;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        text.text = "";
    }

    public void TaskDone()
    {
        GetComponent<Image>().color = Color.yellow;
    }

    public void TaskUnDone()
    {
        GetComponent<Image>().color = Color.white;
    }

    public void SetText(string description)
    {
        text.text = description;
    }

    public void SetObjectTask(ObjectTask objectTask)
    {
        int i = 0;

        if(objectTask.taskIcons == null)
        {
            return;
        }

        //text.text = objectTask.description;

        foreach (Sprite s in objectTask.taskIcons)
        {
            GameObject image = Instantiate(imagePrefab, transform);
            RectTransform imageRectTransform = image.GetComponent<RectTransform>();
            imageRectTransform.anchoredPosition =  new Vector2(-rectTransform.sizeDelta.x /2 + imageRectTransform.sizeDelta.x /2 + (i * size) + marginX, 0 - marginY);
            image.GetComponent<Image>().sprite = s;
            i++;
        }

    }
}
