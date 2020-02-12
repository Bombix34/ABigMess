using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PresentationTriggerCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera cameraToLaunch;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CameraManager.Instance.SetNewCamera(cameraToLaunch);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.Instance.SetNewCamera(cameraToLaunch);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
