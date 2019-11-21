using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]

public class PlayerManager : ObjectManager
{
    public PlayerReglages reglages;

    PlayerInputManager inputs;

    PlayerMovement movement;

    Transform mainCamera;

    Animator animator;

    GameObject interactObject = null;           //raycasted object in front of player

    GameObject grabbedObject = null;            //object currently hold/grabb

    CapsuleCollider playerCollider = null;      // The player collider

    BoxCollider grabbedObjectCollider = null;   // Added collider to the player
    BoxCollider grabbedObjectTrigger = null;    // Added collider to the player

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


    List<InteractObject> raycastedObjects;
    int raycastIndex = 0;

    void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        mainCamera = Camera.main.transform;
        movement = GetComponent<PlayerMovement>();
        movement.Reglages = reglages;
        playerCollider = GetComponent<CapsuleCollider>();

        animator = GetComponentInChildren<Animator>();
        raycastedObjects = new List<InteractObject>();
        ChangeState(new PlayerBaseState(this));
    }

    private void Start()
    {
    }

    private void Update()
    {
        //TEST__________
        UpdateQuackSound();
        //__________
        RaycastObject();
        UpdateGrabbedObject();
        UpdateAnim();
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
        Gizmos.DrawRay(lastRaycastRay);
    }

    public void UpdateMovement()
    {
        movement.DoMove(inputs.GetMovementInput());
    }
    
    private void UpdateAnim()
    {
        if (animator == null)
            return;
        // animator.SetFloat("MoveSpeed", currentVelocity.magnitude / 0.1f);
        animator.SetBool("isRunning", movement.CurrentVelocity.magnitude > 0);
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
                    ResetRaycastedObjects();
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
            grabbedObject.transform.position = Vector3.Lerp(startGrabPosition, bringPosition.transform.position, percentage);
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
        if ((grabbedObject != null && inputs.GetGrabInputDown())||(!movement.IsGrounded() && isGrabbedObjectColliding))
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

    //RAYCAST OBJECTS___________________________________________________________________________________

    public void RaycastObject()
    {
        raycastedObjects.Clear();
        Vector3 testPosition = movement.GetFrontPosition();
        //utiliser Physics.OverlapCapsule plutot que overlapsphere
        Collider[] hitColliders = Physics.OverlapSphere(testPosition, reglages.raycastRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<InteractObject>() !=null)
            {
                raycastedObjects.Add(hitColliders[i].gameObject.GetComponent<InteractObject>());
            }
            i++;
        }
        if (raycastedObjects.Count==0)
        {
            ResetRaycastedObjects();
        }
        else
        {
            if (raycastIndex >= raycastedObjects.Count)
            {
                raycastIndex = 0;
            }
            interactObject = raycastedObjects[raycastIndex].gameObject;
            interactObject.GetComponent<InteractObject>().Highlight();
        }
    }

    public void SwitchRaycastedObject()
    {
        if(inputs.GetSwitchInputDown()&&raycastedObjects.Count>0)
        {
            interactObject.GetComponent<InteractObject>().ResetHighlight();
            raycastIndex++;
            if(raycastIndex>=raycastedObjects.Count)
            {
                raycastIndex = 0;
            }
        }
    }

    private void ResetRaycastedObjects()
    {
        raycastedObjects.Clear();
        raycastIndex = 0;
        interactObject = null;
    }


    public void UpdateQuackSound()
    {
        if(inputs.GetQuackInputDown())
        {
            AkSoundEngine.SetState("MUTE", "up");
        }
        else if(inputs.GetQuackInputUp())
        {
            AkSoundEngine.SetState("MUTE", "down");
        }
    }

 //GET & SET________________________________________________________________________________________

    public PlayerInputManager GetInputManager()
    {
        return inputs;
    }

}
