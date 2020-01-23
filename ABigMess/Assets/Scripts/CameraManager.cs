using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton<CameraManager>
{
    private GameManager manager;

    [SerializeField]
    CameraSettings settings;


    public List<CinemachineVirtualCamera> roomCameras;
    public CinemachineVirtualCamera mainCamera;

    CinemachineFramingTransposer mainCameraTransposer;

    private bool isMainCameraActive = false;

    private void Start()
    {
        manager = GameManager.Instance;
        mainCameraTransposer = mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        ResetCamerasPriority();
        mainCamera.Priority = 10;
        isMainCameraActive = true;
    }

    private void Update()
    {
        UpdateMainCameraZoom();
    }

    private void UpdateMainCameraZoom()
    {
        if (isMainCameraActive)
        {
            mainCameraTransposer.m_CameraDistance = 5f * manager.GetPositionBetweenPlayersAmplitude();
            if (mainCameraTransposer.m_CameraDistance > settings.maxZoomOut)
            {
                mainCameraTransposer.m_CameraDistance = settings.maxZoomOut;
            }
            else if (mainCameraTransposer.m_CameraDistance < settings.maxZoomIn)
            {
                mainCameraTransposer.m_CameraDistance = settings.maxZoomIn;
            }
        }
    }

    /// <summary>
    /// function to switch camera currently used
    /// </summary>
    /// <param name="roomNb">the room nb we want to focus on </param>
    /// <returns>
    /// true if the camera is switch to the room camera
    /// false if the mainCamera is chosen
    /// </returns>
    public bool SwitchCamera(int roomNb)
    {
        ResetCamerasPriority();
        if (GameManager.Instance.PlayerInSameRoom() && roomCameras.Count>0)
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

    /// <summary>
    /// be careful, every camera will be at 0, 
    /// always change one priority after to make sure one cam is not choose at rand
    /// </summary>
    private void ResetCamerasPriority()
    {
        mainCamera.Priority = 0;
        foreach(var cam in roomCameras)
        {
            cam.Priority = 0;
        }
    }

    #region GET/SET

    public bool IsMainCameraActive
    {
        get => isMainCameraActive;
        set
        {
            isMainCameraActive = value;
        }
    }

    #endregion
}
