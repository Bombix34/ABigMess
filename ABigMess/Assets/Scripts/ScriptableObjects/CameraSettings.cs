using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Camera Reglages")]
public class CameraSettings : ScriptableObject
{

    [Header("Zoom settings")]
    [Range(5f, 50f)]
    public float maxZoomIn = 3f;
    [Range(5f, 50f)]
    public float maxZoomOut = 30f;


}
