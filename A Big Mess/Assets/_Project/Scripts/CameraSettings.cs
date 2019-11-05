using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CameraSettings", menuName = "ScriptableObjects/CameraSettings", order = 1)]
public class CameraSettings : ScriptableObject
{

    [Header("PositionSettings")]
    // distance from our target
    // how far in the sky the camera needs to be ?
    // bools for zooming ad smoothfollowing
    // min and max zoom settings
    public float distanceFromTarget = -10;
    public bool allowZoom = true;
    public float zoomSmooth = 100;
    public float zoomStep = 5;
    public float maxZoom = -5;
    public float minZoom = -15;
    public bool smoothFollow = true;
    public float smooth = 0.05f;

    [HideInInspector]
    public float newDistance = -10;

    [Header("OrbitSettings")]
    public float xRotation = -65;
    public float yRotation = -180;
    public bool allowOrbit = true;
    public float yOrbitSmooth = 0.5f;

    [Header("InputSettings")]
    public string MOUSE_ORBIT = "MouseOrbit";
    public string ZOOM = "Mouse ScrollWheel";


}
