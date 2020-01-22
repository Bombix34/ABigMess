using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Text chronoField;

    public void UpdateChronoUI(float currentTime)
    {
        int minutes = (int)(currentTime / 60f);
        int seconds = (int)(currentTime % 60f);
        if(seconds < 10)
        {
            chronoField.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
        }
        else
        {
            chronoField.text ="0"+minutes.ToString()+":"+seconds.ToString();  
        }
    }
}
