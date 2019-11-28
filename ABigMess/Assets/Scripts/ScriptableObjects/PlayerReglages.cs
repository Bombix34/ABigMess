using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="BIGMESS/Players Reglages")]

public class PlayerReglages : ScriptableObject
{
    [Header("Reglages Mouvements")]
    [Tooltip("Vitesse de déplacement du personnage")]
    [Range(0.1f, 5f)]
    public float moveSpeed;

    public float gravity = 9.8f;

    [Tooltip("Vitesse de rotation du personnage")]
    [Range(5f, 50f)]
    public float rotationSpeed;

    [Header("Reglages Raycast des objets")]
    [Range(0.2f, 1f)]
    public float raycastRadius;
    [Range(1f, 3f)]
    public float raycastOffsetPosition;

    [Range(-1f, 1f)]
    public float raycastYPosOffset;

    public ToolSettings noObjectInHandEventsList;
}
