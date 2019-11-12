using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager :Singleton<CameraManager>
{
    public List<CinemachineVirtualCamera> roomCameras;
    public CinemachineVirtualCamera mainCamera;

    public Material hiddenWallMaterial;

    private bool isMainCameraActive = false;
    public bool IsMainCameraActive
    {
        get => isMainCameraActive;
        set
        {
            isMainCameraActive = value;
        }
    }

    public bool SwitchCamera(int roomNb)
        //return true if the camera is switch to the room camera
        //return false if the mainCamera is chosen
    {
        ResetCamerasPriority();
        if (GameManager.Instance.PlayerInSameRoom())
        {
            roomCameras[roomNb].Priority = 10;
            isMainCameraActive = false;
            return true;
        }
        else
        {
            mainCamera.Priority = 10;
            isMainCameraActive = true;
            return false;
        }
    }

    private void ResetCamerasPriority()
        //be careful, every camera will be at 0, 
        //always change one priority after to make sure one cam is not choose at rand
    {
        mainCamera.Priority = 0;
        foreach(var cam in roomCameras)
        {
            cam.Priority = 0;
        }
    }
}
