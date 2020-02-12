using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class CameraManager : Singleton<CameraManager>
{
    private GameManager manager;

    private DepthOfField depthOfFieldLayer;

    [SerializeField]
    CameraSettings settings;


    public List<CinemachineVirtualCamera> roomCameras;
    public CinemachineVirtualCamera mainCamera;

    [Range(0.1f, 10.0f)]
    public float minDistFocus = 5.1f;

    [Range(0.1f,10.0f)]
    public float maxAperture = 2.6f;

    CinemachineFramingTransposer mainCameraTransposer;

    private bool isMainCameraActive = false;

    private float apertureInitValue;

    private void Start()
    {
        manager = GameManager.Instance;
        mainCameraTransposer = mainCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        ResetCamerasPriority();
        mainCamera.Priority = 10;
        isMainCameraActive = true;

        //Init Post Process DoF
        PostProcessVolume volume = Camera.main.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out depthOfFieldLayer);
        apertureInitValue = depthOfFieldLayer.aperture.value;
    }

    private void Update()
    {
        UpdateMainCameraZoom();

        UpdateCameraDoF();
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

    private void UpdateCameraDoF()
    {
        //Focus on player middle point
        float focusDist;
        focusDist = Vector3.Distance(manager.GetPositionBetweenPlayers(), Camera.main.transform.position);
        depthOfFieldLayer.focusDistance.value = focusDist;

        //Change aperture depending on focus distance - Min Dist = 5.1 - Max Dist = 9
        float lerpAperture;
        lerpAperture = (focusDist - minDistFocus) / (9f - minDistFocus);
        lerpAperture = Mathf.Clamp(lerpAperture, 0, 1);
        depthOfFieldLayer.aperture.value = Mathf.Lerp(maxAperture, apertureInitValue, lerpAperture);
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
    public void SetNewCamera(CinemachineVirtualCamera newCam)
    {
        ResetCamerasPriority();
        newCam.m_Priority = 10;
    }

    /// <summary>
    /// be careful, every camera will be at 0, 
    /// always change one priority after to make sure one cam is not choose at rand
    /// </summary>
    private void ResetCamerasPriority()
    {
        mainCamera.Priority = 0;
        if(roomCameras!=null )
        {
            foreach(var cam in roomCameras)
            {
                cam.Priority = 0;
            }
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

    public int CameraCount
    {
        get => roomCameras.Count;
    }

    #endregion
}
