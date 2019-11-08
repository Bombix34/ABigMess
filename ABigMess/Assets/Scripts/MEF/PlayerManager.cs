using System;
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

    GameObject interactObject = null;  //raycasted object in front of player

    GameObject grabbedObject = null;   //object currently hold/grabb

    CapsuleCollider playerCollider = null; // The player collider

    BoxCollider grabbedObjectCollider = null; // Added collider to the player
    BoxCollider grabbedObjectTrigger = null; // Added collider to the player

    [SerializeField]
    GameObject bringPosition;

    Vector3 startGrabPosition; // Lerping with percentage for grabbing an object
    float timeStartedLerping;
    float grabSpeed = 0.06f; // The lesser the speed the faster the grab

    bool isGrabbedObjectColliding;
    bool isPlayerGrounded;

    void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        mainCamera = Camera.main.transform;
        rigidBody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

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
        if (inputs.GetInteractInputDown())
        {
            print("interact");
        }
        if (inputs.GetGrabInputDown())
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

        IsPlayerGrounded();
        if(!isPlayerGrounded && isGrabbedObjectColliding)
        {
            DropBringObject();
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
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawWireSphere(GetFrontPosition(), reglages.raycastRadius);
        Gizmos.DrawRay(GetGroundedRay());
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
            float percentage = timeSinceStarted / grabSpeed;
            //print(timeSinceStarted +  " - " + percentage);
            grabbedObject.transform.position = Vector3.Lerp(startGrabPosition, bringPosition.transform.position, percentage);
            grabbedObject.transform.rotation = bringPosition.transform.rotation;

            if (percentage >= 1.0f) // Once we finished to lerp
            {
                grabbedObject.transform.parent = bringPosition.transform;
                grabbedObjectCollider = gameObject.AddComponent<BoxCollider>();
                grabbedObjectCollider.center = bringPosition.transform.localPosition;
                grabbedObjectCollider.size = grabbedObject.transform.localScale;

                grabbedObjectTrigger = gameObject.AddComponent<BoxCollider>();
                grabbedObjectTrigger.isTrigger = true;
                grabbedObjectTrigger.center = bringPosition.transform.localPosition;
                grabbedObjectTrigger.size = grabbedObject.transform.localScale * 1.2f;

                Destroy(grabbedObject.GetComponent<BoxCollider>());
                reachedPosition = true;
            }
        }
    }

    // COLLIDERS/OBJECTS FUNCTIONS ___________________________________________________________________________________

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
        //Destroy both grabbedObjectTrigger and grabbedObjectCollider
        Destroy(grabbedObjectCollider);
        Destroy(grabbedObjectTrigger);
        isGrabbedObjectColliding = false;
    }

    public void OnCollisionEnter(Collision collision)
    {

    }

    public void OnCollisionStay(Collision collision)
    {

        //print("Player colliding");

    }


    public void OnTriggerStay(Collider other)
    {
        ////if(other == grabbedObjectTrigger)
        print("Object colliding");
        isGrabbedObjectColliding = true;

    }

    public void OnTriggerExit(Collider other)
    {
        isGrabbedObjectColliding = false;
    }

    public void IsPlayerGrounded()
    {
        isPlayerGrounded = Physics.Raycast(GetGroundedRay(), 1.05f);
    }

    //RAYCAST OBJECTS___________________________________________________________________________________

    public void RaycastObject()
    {
        bool isResult = false;
        GameObject raycastObject = null;
        Vector3 testPosition = GetFrontPosition();

        Collider[] hitColliders = Physics.OverlapSphere(testPosition, reglages.raycastRadius);
        //utiliser Physics.OverlapCapsule plutot que overlapsphere
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.tag == "GrabObject")
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
        Vector3 forwardPos = transform.TransformDirection(Vector3.forward) * 0.5f * reglages.raycastOffsetPosition;
        Vector3 testPosition = new Vector3(transform.position.x + forwardPos.x,
            transform.position.y + forwardPos.y + reglages.raycastYPosOffset,
            transform.position.z + forwardPos.z);
        return testPosition;
    }

    public Ray GetGroundedRay()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        return ray;
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
