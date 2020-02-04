using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerMovement : MonoBehaviour
{

    private Rigidbody rigidBody;
    private bool canMove = true;
    private bool canRotate = true;
    private PlayerReglages reglages;
    private Vector3 currentVelocity;

    //to apply a modif to the rotation speed
    private float modifRotationSpeed = 1f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void DoMove(Vector3 playerInputs)
    {
        if (!canMove)
        {
            return;
        }
        Vector3 directionController = playerInputs;
        if (directionController == Vector3.zero)
        {
            currentVelocity = Vector3.zero;
            return;
        }
        //init values
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        Vector3 right = Camera.main.transform.right;

        Vector3 rightMove = right * (10 * playerInputs.x) * Time.deltaTime;
        Vector3 upMove = forward * (10 * playerInputs.z) * Time.deltaTime;
        Vector3 heading = (rightMove + upMove).normalized;

        float amplitude = new Vector2(playerInputs.x, playerInputs.z).magnitude;

        if (canRotate)
        {
            RotateObject(-playerInputs.x, -playerInputs.z);
        }
        currentVelocity = Vector3.zero;
        currentVelocity += heading * amplitude * (reglages.moveSpeed / 5f);
        rigidBody.MovePosition(transform.position + currentVelocity);
    }

    /// <summary>
    /// prevent the player from moving while nothing happened
    /// this strange behavior is due to the arms spline physic
    /// </summary>
    public void PreventPlayerRotation()
    {
        if (currentVelocity == Vector3.zero)
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public Vector3 GetFrontPosition()
    {
        //FONCTION POUR OBTENIR LA POSITION DEVANT LE PERSONNAGE
        //POSITION OU INTERAGIR ET POSER LES OBJETS
        Vector3 forwardPos = transform.TransformDirection(Vector3.forward) * 0.5f * reglages.raycastOffsetPosition;
        Vector3 testPosition = new Vector3(transform.position.x + forwardPos.x,
            transform.position.y + forwardPos.y + reglages.raycastYPosOffset,
            transform.position.z + forwardPos.z);
        return testPosition;
    }

    public void ResetVelocity()
    {
        rigidBody.MovePosition(transform.position);
    }

    #region ROTATION

    //variables for torso rotation system
    const int SIZE_ROTATION_SAVE = 8;
    Quaternion[] bodyRotation = new Quaternion[SIZE_ROTATION_SAVE];
    int indexRotation = 0;
    int indexRotationTorso = 0;
    bool isInitTorsoRotation = false;
    //----------------------------------

    private void RotateObject(float x, float y)
    {
        Vector3 dir = new Vector3(-y, 0, x);
        float diffAngle = transform.rotation.eulerAngles.y - Quaternion.LookRotation(dir).eulerAngles.y;
        if(Mathf.Abs(diffAngle)>120)
        {
            RotateObject(-x, -y);
            return;
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), (reglages.rotationSpeed * 100 * modifRotationSpeed) * Time.deltaTime);
        }
    }

    #endregion

    #region GET/SET

    public Vector3 CurrentVelocity
    {
        get => currentVelocity;
        set
        {
            currentVelocity = value;
        }
    }

    public PlayerReglages Reglages
    {
        set
        {
            reglages = value;
        }
    }

    public Rigidbody Body
    {
        get => rigidBody;
    }

    public bool CanMove
    {
        get => canMove;
        set
        {
            canMove = value;
        }
    }

    public bool CanRotate
    {
        get => canRotate;
        set
        {
            canRotate = value;
        }
    }
    public float ModificationRotationSpeed
    {
        get => modifRotationSpeed;
        set
        {
            modifRotationSpeed = value;
        }
    }

    public Vector3 GetHeadingDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(GetGroundedRay(), 1.05f);
    }

    public Ray GetGroundedRay()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        return ray;
    }

    #endregion
}
