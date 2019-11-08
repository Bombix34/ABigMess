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
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            if (CameraManager.Instance.SwitchCamera(roomNb))
            {
                ModifyWallsMaterial(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().CurrentRoomNb = roomNb;
            ModifyWallsMaterial(CameraManager.Instance.SwitchCamera(roomNb));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CameraManager.Instance.IsMainCameraActive)
                return;
            ModifyWallsMaterial(false);
        }
    }

    private void ModifyWallsMaterial(bool isTransp)
    {
        if(isTransp)
        {
            foreach (var item in wallsToHide)
            {
                item.GetComponent<Renderer>().materials[0]= CameraManager.Instance.hiddenWallMaterial;
            }
        }
        else
        {
            for(int i =0; i < wallsToHide.Count;i++)
            {
              //  item.enabled = true;
             //   wallsToHide[i].enabled = true;
                wallsToHide[i].materials[0] = wallsMaterial[i];
            }
        }
    }
}

