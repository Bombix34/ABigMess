using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTaskPanelTrigger : MonoBehaviour
{
    public GameObject UIManager;

    private void OnTriggerEnter(Collider other)
    {
        if(UIManager == null)
        {
            print("UI manager is required");
            return;
        }
        UIManager.GetComponent<UIManager>().ShowTaskPanel();
    }
}
