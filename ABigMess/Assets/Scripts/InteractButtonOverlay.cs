using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractButtonOverlay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    private Image actionIcon;

    void Awake()
    {
        text.text = Random.Range(1, 99999).ToString();
    }
    
    public void SetText(string text)
    {
        this.text.text = text;
        this.text.color = Color.white;
    }

    public void SetActionIcon(Sprite image)
    {
        if(image==null)
        {
            return;
        }
        this.actionIcon.sprite = image;
    }

    public void SetErrorText(string text)
    {
        this.text.text = text;
        this.text.color = Color.red;
    }
}
