using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void AddImage()
    {

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
}
