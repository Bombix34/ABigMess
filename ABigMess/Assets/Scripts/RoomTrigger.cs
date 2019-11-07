using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    int roomNb = 0;


    [SerializeField]
    List<MeshRenderer> wallsToHide;
    private List<Material> wallsMaterial;

    bool isActive = false;


    private void Start()
    {
        wallsMaterial = new List<Material>();
        foreach(var wall in wallsToHide)
        {
            wallsMaterial.Add(wall.material);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            if (CameraManager.Instance.SwitchCamera(roomNb))
            {
                isActive = true;
                ModifyWallsMaterial(true);
            }
            else
            {
                isActive = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            if (CameraManager.Instance.SwitchCamera(roomNb))
            {
                isActive = true;
                ModifyWallsMaterial(true);
            }
            else
            {
                isActive = false;
            }
        }
    }

    private void ModifyWallsMaterial(bool isTransp)
    {
        if(isTransp)
        {
            if (isActive)
                return;
            foreach(var item in wallsToHide)
            {
                item.material = CameraManager.Instance.hiddenWallMaterial;
            }
        }
        else
        {
            for(int i =0; i < wallsToHide.Count;i++)
            {
                wallsToHide[i].material = wallsMaterial[i];
            }
        }
    }
}

