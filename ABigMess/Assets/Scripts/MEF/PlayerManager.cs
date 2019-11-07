﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]

public class PlayerManager : ObjectManager
{
    public PlayerReglages reglages;

    PlayerInputManager inputs;

    Rigidbody rigidBody;

    Vector3 currentVelocity;

    Transform mainCamera;

    Animator animator;

    GameObject interactObject= null;  //raycasted object in front of player

    GameObject grabbedObject=null;   //object currently hold/grabb

    BoxCollider grabbedObjectCollider = null; // Added collider to the player

    [SerializeField]
    GameObject bringPosition;

    Vector3 startGrabPosition; // Lerping with percentage for grabbing an object
    float timeStartedLerping;


    void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        mainCamera = Camera.main.transform;
        rigidBody = GetComponent<Rigidbody>();

        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
       // ChangeState(new PlayerBaseState(this));
    }

    private void Update()
    {
        RaycastObject();
        UpdateGrabbedObject();
        if(inputs.GetInteractInputDown())
        {
            print("interact");
        }
        if(inputs.GetGrabInputDown())
        {
            if (grabbedObject == null)
            {
                if (interactObject != null)
                {
                    grabbedObject = interactObject;
                    timeStartedLerping = Time.time;
                    //grabbedObject.transform.parent = bringPosition.transform;
                    startGrabPosition = grabbedObject.transform.position;
                    reachedPosition = false;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    interactObject = null;
                    print(grabbedObject);
                }
            }
            else
            {
                DropBringObject();
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
            DoMove();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    private void OnDrawGizmos()
    {

    }

    //MOVEMENT FUNCTIONS______________________________________________________________________________

    bool canMove = true;

    public void Move(bool IsOn)
    {
        canMove = IsOn;
    }

    public void DoMove()
    {
        Vector3 directionController = inputs.GetMovementInput();
        GravitySpeed();
        if (directionController == Vector3.zero)
        {
            currentVelocity = Vector3.zero;
            UpdateAnim();
            return;
        }
        //init values
        Vector3 forward = mainCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        Vector3 right = mainCamera.transform.right;

        Vector3 rightMove = right * (10 * inputs.GetMovementInputX()) * Time.deltaTime;
        Vector3 upMove = forward * (10 * inputs.GetMovementInputY()) * Time.deltaTime;
        Vector3 heading = (rightMove + upMove).normalized;

        float amplitude = new Vector2(inputs.GetMovementInputX(), inputs.GetMovementInputY()).magnitude;

        RotatePlayer(inputs.GetMovementInputY(), -inputs.GetMovementInputX());
        currentVelocity = Vector3.zero;
        currentVelocity += heading * amplitude * (reglages.moveSpeed / 5f);
        rigidBody.MovePosition(transform.position + currentVelocity);
        UpdateAnim();
    }

    public void GravitySpeed()
    {
        //if (!rigidBody.isGrounded)
        //{
            //currentVelocity = Vector3.zero;
            //float gravity = reglages.gravity * Time.deltaTime;
            //currentVelocity = new Vector3(currentVelocity.x, currentVelocity.y - gravity, currentVelocity.z);
            //rigidBody.MovePosition(currentVelocity);
        //}
    }

    public void ResetVelocity()
    {
        rigidBody.MovePosition(transform.position);
        animator.SetFloat("MoveSpeed", 0f);
    }

    private void RotatePlayer(float x, float y)
    {
        Vector3 dir = new Vector3(-y, 0, x);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), (reglages.rotationSpeed * 100) * Time.deltaTime);
    }

    private void UpdateAnim()
    {
        if (animator == null)
            return;
       // animator.SetFloat("MoveSpeed", currentVelocity.magnitude / 0.1f);
       animator.SetBool("isRunning", currentVelocity.magnitude > 0); 
    }


    //Reached position
    public bool reachedPosition = false;
    void UpdateGrabbedObject()
    {
        if (grabbedObject == null)
            return;

        if (!reachedPosition)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentage = timeSinceStarted / 0.06f;
            //print(timeSinceStarted +  " - " + percentage);
            grabbedObject.transform.position = Vector3.Lerp(startGrabPosition, bringPosition.transform.position, percentage);
            grabbedObject.transform.rotation = bringPosition.transform.rotation;

            if(percentage >= 1.0f)
            {
                grabbedObject.transform.parent = bringPosition.transform;
                grabbedObjectCollider = gameObject.AddComponent<BoxCollider>();
                grabbedObjectCollider.center = bringPosition.transform.localPosition;
                Destroy(grabbedObject.GetComponent<BoxCollider>());
                reachedPosition = true;
            }
        }        
    }


    public void JointBroken(JointBreak jointBreak)
    {
        DropBringObject();
    }

    public void DropBringObject()
    {
        grabbedObject.transform.parent = null;
        grabbedObject.AddComponent<BoxCollider>();
        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
        grabbedObject = null;
        Destroy(gameObject.GetComponent<BoxCollider>());
    }

    //RAYCAST OBJECTS___________________________________________________________________________________

    public void RaycastObject()
    {
        bool isResult = false;
        GameObject raycastObject = null;
        Vector3 testPosition = GetFrontPosition();

        Collider[] hitColliders = Physics.OverlapSphere(testPosition, reglages.raycastRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].gameObject.tag=="GrabObject")
            {
                interactObject = hitColliders[i].gameObject;
                isResult = true;
                break;
            }
            i++;
        }
        if (!isResult)
        {
           // if(interactObject!=null)
            //    interactObject.GetComponent<InteractObject>().UpdateFeedback(false);
            interactObject = null;
        }
    }

    /*
    // Adds a cube collider to the player in order for the cube not to go through walls
    void AddHoldCubeCollider()
    {
        holdCubeBoxCollider = gameObject.AddComponent<BoxCollider>();
        Debug.Log("Local Position: " + cubePosition.localPosition);
        holdCubeBoxCollider.center = cubePosition.localPosition;
        Destroy(holdCube.GetComponent<BoxCollider>());
    }

    void RemoveHoldCubeCollider()
    {
        Destroy(holdCubeBoxCollider);
        holdCube.AddComponent<BoxCollider>();
    }
    */

    public Vector3 GetFrontPosition()
    {
        //FONCTION POUR OBTENIR LA POSITION DEVANT LE PERSONNAGE
        //POSITION OU INTERAGIR ET POSER LES OBJETS
        Vector3 forwardPos = transform.TransformDirection(Vector3.forward) * 0.5f;
        Vector3 testPosition = new Vector3(transform.position.x + forwardPos.x,
            transform.position.y + forwardPos.y,
            transform.position.z + forwardPos.z);
        return testPosition;
    }

    public Vector3 GetHeadingDirection()
    {
        return transform.TransformDirection(Vector3.forward);
    }

    //GET & SET________________________________________________________________________________________

    public PlayerInputManager GetInputManager()
    {
        return inputs;
    }
    
}
