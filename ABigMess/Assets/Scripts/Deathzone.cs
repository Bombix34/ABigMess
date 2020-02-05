using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathzone : MonoBehaviour
{
    SceneObjectDatas sceneObjects;

    private void Awake()
    {
        sceneObjects = SceneObjectDatas.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<InteractObject>()!=null)
        {
            InteractObject concernedObj = other.GetComponent<InteractObject>();
            sceneObjects.RemoveObject(concernedObj);
            if(concernedObj.GetComponentInChildren<ObjectZoneArea>()!=null)
            {
                sceneObjects.RemoveZoneArea(concernedObj.GetComponentInChildren<ObjectZoneArea>());
            }
            if(concernedObj.Settings.objectType==ObjectSettings.ObjectType.radio)
            {
                GameManager.Instance.MusicManager.ShutRadio();
            }
            Destroy(other.gameObject);
            GameManager.Instance.TasksManager.UpdateTasksState();
        }
        else if(other.GetComponent<PlayerManager>()!=null)
        {
            GameManager.Instance.ReloadLevel();
        }
    }
}
