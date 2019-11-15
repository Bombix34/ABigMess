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
    
    [SerializeField] int currentRoomNb=0;
    public int CurrentRoomNb
    {
        get => currentRoomNb;
        set
        {
            currentRoomNb = value;
        }
    }

    Ray lastRaycastRay;

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
        ChangeState(new PlayerBaseState(this));
    }

    private void Start()
    {
    }

    private void Update()
    {
        RaycastObject();
        UpdateGrabbedObject();
        IsPlayerGrounded();
        currentState.Execute();
    }

    private void FixedUpdate()
    {
        currentState.FixedExecute();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        //Gizmos.DrawWireSphere(GetFrontPosition(), reglages.raycastRadius);
        //Gizmos.DrawRay(GetGroundedRay());
        //Gizmos.DrawLine(lastRaycastRay.origin, lastRaycastRay.direction);
        Gizmos.DrawRay(lastRaycastRay);
    }

    //MOVEMENT FUNCTIONS______________________________________________________________________________

    bool canMove = true;

    public void Move(bool IsOn)
    {
        canMove = IsOn;
    }

    public void DoMove()
    {
        if (!canMove)
            return;
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
    
    public void TryBringObject()
    {
        if (inputs.GetGrabInputDown())
        {
            if (grabbedObject == null)
            {
                if (interactObject != null)
                {
                    grabbedObject = interactObject;
                    grabbedObject.GetComponent<InteractObject>().Interact();
                    timeStartedLerping = Time.time;
                    //grabbedObject.transform.parent = bringPosition.transform;
                    startGrabPosition = grabbedObject.transform.position;
                    reachedPosition = false;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    interactObject = null;
                    ChangeState(new PlayerBringState(this, grabbedObject.GetComponent<InteractObject>()));
                }
            }
        }
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
            //grabbedObject.transform.rotation = bringPosition.transform.rotation;

            Vector3 forward = grabbedObject.transform.TransformDirection(Vector3.forward);
            Vector3 toOther = transform.position - grabbedObject.transform.position;
            lastRaycastRay = new Ray( grabbedObject.transform.position, transform.position - grabbedObject.transform.position);
            //RaycastHit hit;
            //Physics.Raycast(lastRaycastRay, out hit, 10f);
            //Destroy(hit.transform.gameObject);

            print(Vector3.Dot(forward, toOther));

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
    
    
    public void DropBringObject()
    {
        if ((grabbedObject != null && inputs.GetGrabInputDown())||(!isPlayerGrounded && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            grabbedObject.AddComponent<BoxCollider>();
            grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
            grabbedObject = null;
            //Destroy both grabbedObjectTrigger and grabbedObjectCollider
            Destroy(grabbedObjectCollider);
            Destroy(grabbedObjectTrigger);
            isGrabbedObjectColliding = false;
            ChangeState(new PlayerBaseState(this));
        }
    }


 // COLLIDERS/OBJECTS FUNCTIONS ___________________________________________________________________________________

    public void OnCollisionEnter(Collision collision)
    {

    }

    public void OnCollisionStay(Collision collision)
    {
    }


    public void OnTriggerStay(Collider other)
    {
        ////if(other == grabbedObjectTrigger)
        //print("Object colliding");
        if (other.CompareTag("GrabObject"))
        {
            isGrabbedObjectColliding = true;
        }

    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabObject"))
        {
            isGrabbedObjectColliding = false;
        }
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
                if (hitColliders[i].gameObject.GetComponent<InteractObject>() == null)
                    return;
                else
                    hitColliders[i].gameObject.GetComponent<InteractObject>().Highlight();
                isResult = true;
                break;
            }
            i++;
        }
        if (!isResult)
        {
            interactObject = null;
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
