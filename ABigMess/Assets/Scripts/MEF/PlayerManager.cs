using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerRenderer))]

public class PlayerManager : ObjectManager
{
    public PlayerReglages reglages;

    private PlayerInputManager inputs;

    [SerializeField]
    private Animator animator;

    private PlayerMovement movement;
    private PlayerRenderer renderer;

    private GameObject interactObject = null;           //raycasted object in front of player
    private GameObject grabbedObject = null;            //object currently hold/grabb

    private CapsuleCollider playerCollider = null;      // The player collider

    private BoxCollider grabbedObjectCollider = null;   // Added collider to the player
    private BoxCollider grabbedObjectTrigger = null;    // Added collider to the player

    [SerializeField]
    private GameObject bringPosition;

    [SerializeField]
    private int currentRoomNb = 0;

    private Ray lastRaycastRay;

    private Vector3 startGrabPosition; // Lerping with percentage for grabbing an object
    private float timeStartedLerping;
    private float grabSpeed = 0.03f; // The lesser the speed the faster the grab

    private bool isGrabbedObjectColliding;

    private List<InteractObject> raycastedObjects;
    private int raycastIndex = 0;

    void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        movement = GetComponent<PlayerMovement>();
        renderer = GetComponent<PlayerRenderer>();
        movement.Reglages = reglages;
        playerCollider = GetComponent<CapsuleCollider>();
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
        movement.PreventPlayerRotation();
        currentState.Execute();
        UpdateAnimation();
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
      //  Gizmos.DrawRay(lastRaycastRay);
        Gizmos.DrawWireSphere(movement.GetFrontPosition(), reglages.raycastRadius);
        Gizmos.DrawWireSphere(new Vector3(movement.GetFrontPosition().x, movement.GetFrontPosition().y * 1.3f, movement.GetFrontPosition().z),reglages.raycastRadius);
        */
    }

    public void UpdateMovement()
    {
        movement.DoMove(inputs.GetMovementInput());
    }

    public void UpdateAnimation()
    {
        animator.SetFloat("MovementSpeed", inputs.GetMovementInput().magnitude );
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
        if (inputs.GetInteractInputDown())
        {
            if (interactObject == null)
            {
                return;
            }
            //Interact object is an object near the player that he wants to interact with
            interactObject.GetComponent<InteractObject>().Interact(this);
        }
    }
    #endregion

    #region BRING_DROP_SYSTEM

    public bool reachedPosition = false;

    public void TryBringObject()
    {
        if (inputs.GetGrabInputDown())
        {
            if (grabbedObject == null && interactObject != null)
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
            interactObject = null;
            movement.ResetTorso();
            //renderer.AttachHandToObject();
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

            //Update position according to weight
            ObjectSettings.ObjectWeight weight = ObjectSettings.ObjectWeight.light;
            if (grabbedObject.GetComponent<InteractObject>().Settings == null)
            {
                Debug.LogError("Define settings for the object: " + grabbedObject.name);
            }
            else
            {
                weight = grabbedObject.GetComponent<InteractObject>().Settings.weightType;
            }

            switch (weight)
            {
                case ObjectSettings.ObjectWeight.heavy:
                    movement.CanMove = false;
                    renderer.AttachHandToObject();
                    reachedPosition = true;
                    break;
                default:
                    grabbedObject.transform.position = Vector3.Lerp(startGrabPosition, bringPosition.transform.position, percentage);
                    grabbedObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbedObject.GetComponent<InteractObject>().Rotation);
                    if (percentage >= 1.0f) // Once we finished to lerp
                    {
                        grabbedObject.transform.parent = bringPosition.transform;
                        renderer.AttachHandToObject();
                        reachedPosition = true;
                    }
                    break;
            }

        }
    }

    public void SwitchGrabbedObject()
    {
        //Update position according to weight
        ObjectSettings.ObjectWeight weight = ObjectSettings.ObjectWeight.light;
        if (interactObject.GetComponent<InteractObject>().Settings == null)
        {
            Debug.LogError("Define settings for the object: " + grabbedObject.name);
        }
        else
        {
            weight = interactObject.GetComponent<InteractObject>().Settings.weightType;
        }
        switch (weight)
        {
            case ObjectSettings.ObjectWeight.heavy:
                break;
            default:
                // Grabbed Object is exchanged
                // Interact Object becomes the object holded by the player
                Vector3 switchObjectPosition = interactObject.transform.position;
                interactObject.transform.position = grabbedObject.transform.position;
                grabbedObject.transform.position = switchObjectPosition;

                grabbedObject.transform.parent = null;
                grabbedObject.GetComponent<InteractObject>().Dropdown();

                isGrabbedObjectColliding = false;
                grabbedObject.transform.parent = null;

                GrabInteractObject();
                break;
        }
    }

    /// <summary>
    /// If no object is highlight, drop the object on the ground
    /// else switch the object in hand with the highlighted one
    /// </summary>
    public void DropBringObject()
    {
        if (inputs.GetGrabInputDown() && grabbedObject != null && interactObject != null )
        {
            SwitchGrabbedObject();
        }
        else
        if ((inputs.GetGrabInputDown() && grabbedObject != null) || (!movement.IsGrounded() && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            renderer.DetachHand();
            grabbedObject.GetComponent<InteractObject>().Dropdown();
            grabbedObject = null;
            isGrabbedObjectColliding = false;
            movement.CanMove = true;
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
        Vector3 capsuleUpPosition = new Vector3(testPosition.x, testPosition.y * 1.3f, testPosition.z);
        Collider[] hitColliders = Physics.OverlapCapsule(testPosition, capsuleUpPosition, reglages.raycastRadius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject.GetComponent<InteractObject>() != null)
            {
                // A ray that verifies that we are not rycasting through a wall
                GameObject raycastedObject= hitColliders[i].gameObject;
                RaycastHit hit;
                Physics.Raycast(transform.position, raycastedObject.transform.position - transform.position, out hit, 5f);

                if (!hit.collider.CompareTag("Wall")) // If we are not going through a wall
                {
                    if (grabbedObject != null && raycastedObject == grabbedObject)
                    {
                        return;
                    }
                    raycastedObjects.Add(raycastedObject.GetComponent<InteractObject>());
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
            if (interactObject != grabbedObject) // Never highlight an object in my hands
            {
                if (grabbedObject == null)
                {
                    interactObject.GetComponent<InteractObject>().Highlight(reglages.noObjectInHandEventsList);
                }
                else
                {
                    interactObject.GetComponent<InteractObject>().Highlight(grabbedObject);
                }
            }
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

    public GameObject GrabbedObject
    {
        get => grabbedObject;
    }

    public PlayerRenderer Renderer
    {
        get => renderer;
    }

    #endregion

}
