using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody rigidBody;
    bool canMove = true;
    bool canRotate = true;
    bool canRotateTorso = true;
    PlayerReglages reglages;
    Vector3 currentVelocity;

    [SerializeField]
    GameObject torso;

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
            RotateTorso(false);
            return;
        }
        //init values
        Vector3 forward =Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        Vector3 right = Camera.main.transform.right;

        Vector3 rightMove = right * (10 * playerInputs.x) * Time.deltaTime;
        Vector3 upMove = forward * (10 * playerInputs.z) * Time.deltaTime;
        Vector3 heading = (rightMove + upMove).normalized;

        float amplitude = new Vector2(playerInputs.x, playerInputs.z).magnitude;

        if(canRotate)
        {
            RotatePlayer(playerInputs.z, -playerInputs.x);
        }
        currentVelocity = Vector3.zero;
        rigidBody.velocity = currentVelocity;
        currentVelocity += heading * amplitude * (reglages.moveSpeed/5f);
        rigidBody.MovePosition(transform.position + currentVelocity);

        print(rigidBody.velocity);
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

    #region ROTATION

    //variables for torso rotation system
    const int SIZE_ROTATION_SAVE = 8;
    Quaternion[] bodyRotation = new Quaternion[SIZE_ROTATION_SAVE];
    int indexRotation = 0;
    int indexRotationTorso = 0;
    bool isInitTorsoRotation = false;
    //----------------------------------

    private void RotatePlayer(float x, float y)
    {
        Vector3 dir = new Vector3(-y, 0, x);
        SaveRotation();
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), (reglages.rotationSpeed * 100) * Time.deltaTime);
        RotateTorso(true);
    }

    private void RotateTorso(bool isMoving)
    {
        if(!canRotateTorso)
        {
            return;
        }
        if(isMoving)
        {
            if (indexRotationTorso < SIZE_ROTATION_SAVE / 2 && !isInitTorsoRotation)
            {
                torso.transform.rotation = Quaternion.RotateTowards(torso.transform.rotation, bodyRotation[0], (reglages.rotationSpeed * 90) * Time.deltaTime);
            }
            else
            {
                torso.transform.rotation = Quaternion.RotateTowards(torso.transform.rotation, bodyRotation[indexRotationTorso % SIZE_ROTATION_SAVE], (reglages.rotationSpeed * 90) * Time.deltaTime);
            }
        }
        else
        {
            torso.transform.rotation = Quaternion.RotateTowards(torso.transform.rotation, transform.rotation, (reglages.rotationSpeed * 90) * Time.deltaTime);
        }
    }

    public void ResetTorso()
    {
        torso.transform.rotation = transform.rotation;
    }

    private void SaveRotation()
    {
        bodyRotation[indexRotation] = transform.rotation;
        indexRotation++;
        indexRotationTorso++;
        if (indexRotation == SIZE_ROTATION_SAVE)
        {
            indexRotation = 0;
        }
        if(indexRotationTorso % SIZE_ROTATION_SAVE == 0)
        {
            isInitTorsoRotation = true;
            indexRotationTorso = 0;
        }
    }

    #endregion

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
      //  animator.SetFloat("MoveSpeed", 0f);
    }

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

    public bool CanMove
    {
        get => canMove;
        set
        {
            canMove = value;
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

    public bool CanRotate
    {
        get => canRotate;
        set
        {
            canRotate = value;
        }
    }

    public bool CanRotateTorso
    {
        set
        {
            canRotateTorso = value;
        }
    }

    #endregion
}
