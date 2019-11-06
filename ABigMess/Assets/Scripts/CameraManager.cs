using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager :Singleton<CameraManager>
{
    public List<CinemachineVirtualCamera> cameras;
    public CinemachineVirtualCamera largeCamera;

    public void SwitchCamera(int roomNb)
    {
        ResetCamerasPriority();
        if (GameManager.Instance.PlayerInSameRoom())
            cameras[roomNb].Priority = 10;
        else
            largeCamera.Priority = 10;
    }

    private void ResetCamerasPriority()
        //be careful, every camera will be at 0, 
        //always change one priority after to make sure one cam is not choose at rand
    {
        largeCamera.Priority = 0;
        foreach(var cam in cameras)
        {
            cam.Priority = 0;
        }
    }
}
