using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideViewCamera : MonoBehaviour
{

    public CameraSettings cameraSettings;

    Vector3 destination = Vector3.zero;
    Vector3 camVelocity = Vector3.zero;
    Vector3 currentMousePosition = Vector3.zero;
    Vector3 previousMousePosition = Vector3.zero;
    float mouseOrbitInput, zoomInput;

    public Transform target;

    void Start()
    {
        //setting camera target
        SetCameraTarget(target);

        if (target)
        {
            MoveToTarget();
        }
    }

    public void SetCameraTarget(Transform t)
    {
        // if we want to set a new target at runtime
        target = t;

        if(target == null)
        {
            Debug.LogError("Camera needs target");
        }

    }

    void GetInput()
    {
        // filling the values for our input variables
        mouseOrbitInput = Input.GetAxisRaw(cameraSettings.MOUSE_ORBIT);
        zoomInput = Input.GetAxisRaw(cameraSettings.ZOOM);
    }

    void Update()
    {
        // getting input
        // zooming
        GetInput();
        if (cameraSettings.allowZoom)
        {
            ZoomInOnTarget();
        }
    }

    void FixedUpdate()
    {
        // movetotarget
        // lookattarget
        // orbit
        if (target)
        {
            MoveToTarget();
            LookAtTarget();
            MouseOrbitTarget();
        }
    }

    void MoveToTarget()
    {
        // hadling getting our camera to its destination position
        destination = target.position;
        destination += Quaternion.Euler(cameraSettings.xRotation, cameraSettings.yRotation, 0) * -Vector3.forward * cameraSettings.distanceFromTarget;

        if (cameraSettings.smoothFollow)
        {
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref camVelocity, cameraSettings.smooth);
        } else
        {
            transform.position = destination;
        }
    }

    void LookAtTarget()
    {
        // handling getting our camera to look at the target at all times
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = targetRotation;

    }

    void MouseOrbitTarget()
    {
        // geting the camera t orbit around our character
        previousMousePosition = currentMousePosition;
        currentMousePosition = Input.mousePosition;

        if(mouseOrbitInput > 0)
        {
            cameraSettings.yRotation += (currentMousePosition.x - previousMousePosition.x) * cameraSettings.yOrbitSmooth;
        }
    }

    void ZoomInOnTarget()
    {
        // modifying the distancefromtarget to be closer or further away from our target
        cameraSettings.newDistance += cameraSettings.zoomStep * zoomInput;

        cameraSettings.distanceFromTarget = Mathf.Lerp(cameraSettings.distanceFromTarget, cameraSettings.newDistance, cameraSettings.zoomSmooth * Time.deltaTime);

        if(cameraSettings.distanceFromTarget > cameraSettings.maxZoom)
        {
            cameraSettings.distanceFromTarget = cameraSettings.maxZoom;
        }

        if (cameraSettings.distanceFromTarget < cameraSettings.minZoom)
        {
            cameraSettings.distanceFromTarget = cameraSettings.minZoom;
        }
    }
}
