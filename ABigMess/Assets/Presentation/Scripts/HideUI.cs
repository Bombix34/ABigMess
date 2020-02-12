using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUI : MonoBehaviour
{
    void Start()
    {
        GetComponent<UIManager>().Hide();
    }
}
