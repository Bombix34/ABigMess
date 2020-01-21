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
    
    private float grabSpeed = 0.03f; // The lesser the speed the faster the grab

    private bool isGrabbedObjectColliding;

    private List<InteractObject> raycastedObjects;
    private int raycastIndex = 0;

    private void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        movement = GetComponent<PlayerMovement>();
        renderer = GetComponent<PlayerRenderer>();
        movement.Reglages = reglages;
        playerCollider = GetComponent<CapsuleCollider>();
        raycastedObjects = new List<InteractObject>();
        ChangeState(new PlayerBaseState(this));
    }

    private void Update()
    {
        //TEST__________
        UpdateQuackSound();
        //__________
        RaycastObject();
        movement.PreventPlayerRotation();
        currentState.Execute();
        renderer.UpdateAnimation(inputs.GetMovementInput().magnitude);
    }

    private void FixedUpdate()
    {
        currentState.FixedExecute();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    public void UpdateMovement()
    {
        movement.DoMove(inputs.GetMovementInput());
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
            if(interactObject.GetComponent<InteractObject>().GetObjectWeight==ObjectSettings.ObjectWeight.immobile)
            {
                return;
            }
            grabbedObject = interactObject;
            grabbedObject.GetComponent<InteractObject>().Grab(this.gameObject);
            ResetRaycastedObjects();
            interactObject = null;
            movement.ResetTorso();
            bool isGrabOneHand = grabbedObject.GetComponent<InteractObject>().Settings.isOneHandedCarrying;
            if(isGrabOneHand)
            {
                AttachGrabbedObjectOneHanded();
            }
            else
            {
                AttachGrabbedObjectTwoHanded();
            }
            ChangeState(new PlayerBringState(this, grabbedObject.GetComponent<InteractObject>()));
        }
    }

    /// <summary>
    /// function to take the object in hand
    /// </summary>
    private void AttachGrabbedObjectTwoHanded()
    {
        if (grabbedObject == null)
        {
            return;
        }
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
        bool reachedPosition=false;
        while (!reachedPosition)
        {
            Vector3 dirVector = bringPosition.transform.position - grabbedObject.transform.position;
            switch (weight)
            {
                case ObjectSettings.ObjectWeight.heavy:
                    movement.CanMove = false;
                    renderer.AttachHandToObject();
                    reachedPosition = true;
                    break;
                default:
                    grabbedObject.transform.position += dirVector.normalized * grabSpeed;
                    grabbedObject.transform.rotation = transform.rotation * Quaternion.Euler(grabbedObject.GetComponent<InteractObject>().Rotation);
                    if (dirVector.magnitude < 0.1f)
                    {
                        grabbedObject.transform.parent = bringPosition.transform;
                        renderer.AttachHandToObject();
                        reachedPosition = true;
                    }
                    break;
            }

        }
    }

    private void AttachGrabbedObjectOneHanded()
    {
        grabbedObject.transform.position = renderer.RightArm.HandPosition.position;
        grabbedObject.transform.parent = renderer.RightArm.HandPosition;
        grabbedObject.AddComponent<FixedJoint>();
        grabbedObject.GetComponent<FixedJoint>().connectedBody = renderer.RightArm.HandPosition.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// We don't use that anymore
    /// </summary>
    private void SwitchGrabbedObject()
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
                grabbedObject.GetComponent<InteractObject>().Dropdown(this.gameObject);

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
        /*
        if (inputs.GetGrabInputDown() && grabbedObject != null && interactObject != null )
        {
            SwitchGrabbedObject();
        }
        else
        */
        if ((inputs.GetGrabInputDown() && grabbedObject != null) || (!movement.IsGrounded() && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            renderer.DetachHand();
            grabbedObject.GetComponent<InteractObject>().Dropdown(this.gameObject);
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

                if (hit.collider!=null && !hit.collider.CompareTag("Wall")) // If we are not going through a wall
                {
                    if (raycastedObject != grabbedObject)
                    {
                        raycastedObjects.Add(raycastedObject.GetComponent<InteractObject>());
                    }
                }
            }
            ++i;
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

    public PlayerInputManager Inputs
    {
        get => inputs;
    }

    public PlayerReglages Reglages
    {
        get => reglages;
    }

    public PlayerMovement Movement
    {
        get => movement;
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
