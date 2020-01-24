using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BIGMESS/Outline objects settings")]
public class OutlineSettings : ScriptableObject
{
    public AnimationCurve outlineAnimation;

    [SerializeField]
    [Range(1, 15)]
    public float outlineWidth = 8;

    [SerializeField]
    [Range(1, 10)]
    public float outlineSpeed = 2;
}
