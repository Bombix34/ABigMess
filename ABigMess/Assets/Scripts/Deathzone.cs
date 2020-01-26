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
            sceneObjects.RemoveObject(other.GetComponent<InteractObject>());
            Destroy(other.gameObject);
            GameManager.Instance.TasksManager.UpdateTasksState();
        }
    }
}
