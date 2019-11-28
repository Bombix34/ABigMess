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

   // Transform mainCamera;

    Animator animator;

    GameObject interactObject = null;           //raycasted object in front of player

    GameObject grabbedObject = null;            //object currently hold/grabb

    CapsuleCollider playerCollider = null;      // The player collider

    BoxCollider grabbedObjectCollider = null;   // Added collider to the player
    BoxCollider grabbedObjectTrigger = null;    // Added collider to the player

    [SerializeField]
    GameObject bringPosition;

    [SerializeField] int currentRoomNb = 0;

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
        /*
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawRay(lastRaycastRay);
        Gizmos.DrawWireSphere(movement.GetFrontPosition(), reglages.raycastRadius);
        Gizmos.DrawWireSphere(new Vector3(movement.GetFrontPosition().x, movement.GetFrontPosition().y * 1.3f, movement.GetFrontPosition().z),reglages.raycastRadius);
        */    
    }

    public void UpdateMovement()
    {
        movement.DoMove(inputs.GetMovementInput());
    }

    private void UpdateAnim()
    {
        if (animator == null)
        {
            return;
        }
        // animator.SetFloat("MoveSpeed", currentVelocity.magnitude / 0.1f);
        animator.SetBool("isRunning", movement.CurrentVelocity.magnitude > 0);
    }

    public void UpdateQuackSound()
    {
        if (inputs.GetQuackInputDown())
        {
            AkSoundEngine.SetState("MUTE", "up");
        }
        else if (inputs.GetQuackInputUp())
        {
            AkSoundEngine.SetState("MUTE", "down");
        }
    }

    #region INTERACTION_SYSTEM
    public void TryInteraction()
    {
        if(inputs.GetInteractInputDown())
        {
            if (interactObject == null)
                return;
            interactObject.GetComponent<InteractObject>().Interact(grabbedObject);
        }
    }
    #endregion

    #region BRING_DROP_SYSTEM

    public bool reachedPosition = false;

    public void TryBringObject()
    {
        if (inputs.GetGrabInputDown())
        {
            if (grabbedObject == null)
            {
                GrabInteractObject();
            }
        }
    }

    /// <summary>
    /// Grabbed object points to a newly found interactObject by raycast. 
    /// The object has it's physics disabled. They come back DropBringObject
    /// Changes the state of the player to bring state
    /// </summary>
    void GrabInteractObject()
    {
        if (interactObject != null)
        {
            grabbedObject = interactObject;
            grabbedObject.GetComponent<InteractObject>().Grab();
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

    void UpdateGrabbedObject()
    {
        if (grabbedObject == null)
        {
            return;
        }
        if (!reachedPosition)
        {
            float timeSinceStarted = Time.time - timeStartedLerping;
            float percentage = timeSinceStarted / grabSpeed;
            grabbedObject.transform.position = Vector3.Lerp(startGrabPosition, bringPosition.transform.position, percentage);
            grabbedObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbedObject.GetComponent<InteractObject>().Rotation);
            if (percentage >= 1.0f) // Once we finished to lerp
            {
                SetupCollidersAndTriggers(grabbedObject);
                grabbedObject.transform.parent = bringPosition.transform;
                reachedPosition = true;
            }
        }
    }

    public void SetupCollidersAndTriggers(GameObject objectToSetup)
    {
        bringPosition.transform.rotation = objectToSetup.transform.rotation;

        grabbedObjectCollider = bringPosition.AddComponent<BoxCollider>();
        grabbedObjectCollider.size = objectToSetup.transform.localScale;

        grabbedObjectTrigger = bringPosition.AddComponent<BoxCollider>();
        grabbedObjectTrigger.isTrigger = true;
        grabbedObjectTrigger.size = objectToSetup.transform.localScale * 1.2f;

        Destroy(objectToSetup.GetComponent<BoxCollider>());
    }

    public void ResetPlayerColliders()
    {
        if (grabbedObjectCollider != null)
        {
            Destroy(grabbedObjectCollider);
        }

        if (grabbedObjectTrigger != null)
        {
            Destroy(grabbedObjectTrigger);
        }
    }

    public void SwitchGrabbedObject()
    {
        if (inputs.GetSwitchInputDown())
        {
            // We switch interactObject with grabbedObject
            if (grabbedObject != null && interactObject != null)
            {
                // Grabbed Object is exchanged so it gains back it's box collider
                // Interact Object becomes the object holded by the player
                Vector3 switchObjectPosition = interactObject.transform.position;

                interactObject.GetComponent<Rigidbody>().isKinematic = true;

                interactObject.transform.position = grabbedObject.transform.position;
                grabbedObject.transform.position = switchObjectPosition;

                grabbedObject.transform.parent = null;
                grabbedObject.AddComponent<BoxCollider>();
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;

                ResetPlayerColliders();
                SetupCollidersAndTriggers(interactObject);


                isGrabbedObjectColliding = false;

                // Reset interact object rotation
                interactObject.transform.rotation = transform.rotation * Quaternion.Euler(interactObject.GetComponent<InteractObject>().Rotation); 
                grabbedObject.transform.parent = null;
                grabbedObject = interactObject;

               
                grabbedObject.transform.parent = bringPosition.transform;
                grabbedObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbedObject.GetComponent<InteractObject>().Rotation);
                

                ChangeState(new PlayerBringState(this, grabbedObject.GetComponent<InteractObject>()));
            }
        }
    }

    public void DropBringObject()
    {
        if ((grabbedObject != null && inputs.GetGrabInputDown()) || (!movement.IsGrounded() && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            grabbedObject.GetComponent<InteractObject>().Dropdown();
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

    #endregion

    #region COLLISION_SYSTEM

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

    #endregion

    #region RAYCAST_SYSTEM

    public void RaycastObject()
    {
        raycastedObjects.Clear();
        Vector3 testPosition = movement.GetFrontPosition();
        Vector3 capsuleUpPosition = new Vector3(testPosition.x, testPosition.y*1.3f, testPosition.z);
        Collider[] hitColliders = Physics.OverlapCapsule(testPosition,capsuleUpPosition,reglages.raycastRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<InteractObject>() != null)
            {
                // A ray that verifies that we are not rycasting through a wall
                RaycastHit hit;
                Physics.Raycast(transform.position, hitColliders[i].transform.position - transform.position, out hit, 5f);

                if (!hit.collider.CompareTag("Wall")) // If we are not going through a wall
                {
                    raycastedObjects.Add(hitColliders[i].gameObject.GetComponent<InteractObject>());
                }
            }
            i++;
        }
        if (raycastedObjects.Count == 0)
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
        if (inputs.GetSwitchInputDown() && raycastedObjects.Count > 0)
        {
            interactObject.GetComponent<InteractObject>().ResetHighlight();
            raycastIndex++;
            if (raycastIndex >= raycastedObjects.Count)
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

    #endregion

    #region GET/SET

    public PlayerInputManager GetInputManager()
    {
        return inputs;
    }

    public int CurrentRoomNb
    {
        get => currentRoomNb;
        set
        {
            currentRoomNb = value;
        }
    }

    #endregion

}
