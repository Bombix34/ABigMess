using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager :Singleton<CameraManager>
{
    public List<CinemachineVirtualCamera> roomCameras;
    public CinemachineVirtualCamera mainCamera;

    public void SwitchCamera(int roomNb)
    {
        ResetCamerasPriority();
        if (GameManager.Instance.PlayerInSameRoom())
            roomCameras[roomNb].Priority = 10;
        else
            mainCamera.Priority = 10;
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
