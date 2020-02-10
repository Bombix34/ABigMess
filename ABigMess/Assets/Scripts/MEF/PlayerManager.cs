using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerInputManager))]
[RequireComponent(typeof(PlayerRenderer))]

public class PlayerManager : ObjectManager
{
    public PlayerReglages reglages;

    private PlayerInputManager inputs;

    private PlayerMovement movement;
    private PlayerRenderer renderer;
    private PlayerRaycast raycast;

    private GameObject interactObject = null;           //raycasted object in front of player
    private GameObject grabbedObject = null;            //object currently hold/grabb

    private CapsuleCollider playerCollider;      // The player collider

    [SerializeField]
    private GameObject bringPosition;

    [SerializeField]
    private int currentRoomNb = -1;
    
    private float grabSpeed = 0.03f; // The lesser the speed the faster the grab

    private bool isGrabbedObjectColliding;

    private void Awake()
    {
        inputs = GetComponent<PlayerInputManager>();
        movement = GetComponent<PlayerMovement>();
        renderer = GetComponent<PlayerRenderer>();
        raycast = GetComponentInChildren<PlayerRaycast>();
        movement.Reglages = reglages;
        playerCollider = GetComponent<CapsuleCollider>();
        ChangeState(new PlayerBaseState(this));
    }

    private void Update()
    {
        UpdateQuackSound();
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
            if (MusicManager.Instance.GetSoundManager() != null)
            {
                MusicManager.Instance.GetSoundManager().QuackSound(inputs.playerId + 1);
            }
            renderer.QuackAnim();
        }
    }

    private void OnDrawGizmos()
    {
        if(Application.isPlaying)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.75f);
            Gizmos.DrawSphere(movement.GetFrontPosition(), reglages.raycastRadius);
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
            GameManager.Instance.TasksManager.UpdateTasksState();
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
            if (MusicManager.Instance.GetSoundManager() != null)
            {
                MusicManager.Instance.GetSoundManager().PlayGrabObjectSound();
            }
            InteractObject obj = grabbedObject.GetComponent<InteractObject>();
            raycast.RemoveFromRaycast(obj);
            if (obj.AttachedPlayersCount > 0 && obj.Settings.weightType!=ObjectSettings.ObjectWeight.heavy)
            {
                obj.DetachPlayer();
            }
            obj.Grab(this.gameObject);
            interactObject = null;
            movement.ResetTorso();
            bool isGrabOneHand = obj.Settings.isOneHandedCarrying;
            if(isGrabOneHand)
            {
                AttachGrabbedObjectOneHanded();
            }
            else
            {
                AttachGrabbedObjectTwoHanded();
            }
            ChangeState(new PlayerBringState(this, obj));
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

    public void AttachGrabbedObjectOneHanded()
    {
        grabbedObject.transform.position = renderer.RightArm.HandPosition.position;
        grabbedObject.transform.parent = renderer.RightArm.HandPosition;
        grabbedObject.AddComponent<FixedJoint>();
        grabbedObject.GetComponent<FixedJoint>().connectedBody = renderer.RightArm.HandPosition.GetComponent<Rigidbody>();
    }

    /// <summary>
    /// drop the object on the ground
    /// </summary>
    public void DropBringObject()
    {
        if ((inputs.GetGrabInputDown() && grabbedObject != null) || (!movement.IsGrounded() && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            if(grabbedObject.GetComponent<FixedJoint>()!=null)
            {
                Destroy(grabbedObject.GetComponent<FixedJoint>());
            }
            renderer.DetachHand();
            if (MusicManager.Instance.GetSoundManager() != null)
            {
                MusicManager.Instance.GetSoundManager().PlayReleaseObjectSound();
            }
            grabbedObject.GetComponent<InteractObject>().Dropdown(this.gameObject);
            grabbedObject = null;
            isGrabbedObjectColliding = false;
            movement.CanMove = true;
            ChangeState(new PlayerBaseState(this));
        }
    }

    /// <summary>
    /// this function help forcing detach player
    /// </summary>
    /// <param name="isForcing"></param>
    public void DropBringObject(bool isForcing)
    {
        if ((isForcing)||(inputs.GetGrabInputDown() && grabbedObject != null) || (!movement.IsGrounded() && isGrabbedObjectColliding))
        {
            grabbedObject.transform.parent = null;
            if (grabbedObject.GetComponent<FixedJoint>() != null)
            {
                Destroy(grabbedObject.GetComponent<FixedJoint>());
            }
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
        if (raycast.GetRaycastedObjectsCount() == 0)
        {
            interactObject = null;
        }
        else
        {
            if(interactObject!=null)
            {
                interactObject.GetComponent<InteractObject>().ResetHighlight();
            }
            interactObject = raycast.GetUpperRaycastedObject().gameObject;
            if (interactObject != grabbedObject) // Never highlight an object in my hands
            {
                InteractObject concernedObject = interactObject.GetComponent<InteractObject>();
                //if i don't have any object in hand
                if (grabbedObject == null)
                {
                    //if i raycast a tool
                    if (concernedObject.Settings.IsTool())
                    {
                        concernedObject.SetHighlightColor(renderer.HighlightToolsColor);
                        concernedObject.Highlight(reglages.noObjectInHandEventsList);
                    }
                    //if i raycast an immobile object with casual interaction
                    else if (concernedObject.Settings.weightType == ObjectSettings.ObjectWeight.immobile)
                    {
                        if (reglages.noObjectInHandEventsList.IsInteractionExisting(concernedObject.Settings.objectType))
                        {
                            concernedObject.SetHighlightColor(renderer.HighlightToolsColor);
                            concernedObject.Highlight(reglages.noObjectInHandEventsList);
                            concernedObject.SetUIActionIcon(reglages.noObjectInHandEventsList.ActionIcon);
                        }
                    }
                    //if i raycast any normal object
                    else
                    {
                        concernedObject.SetHighlightColor(renderer.HighlightObjectsColor);
                        concernedObject.Highlight(reglages.noObjectInHandEventsList);
                        if (reglages.noObjectInHandEventsList.IsInteractionExisting(concernedObject.Settings.objectType))
                        {
                            concernedObject.SetUIActionIcon(reglages.noObjectInHandEventsList.ActionIcon);
                        }
                    }
                }
                //if i have an object in hand
                else
                {
                    InteractObject objectInHand = grabbedObject.GetComponent<InteractObject>();
                    if(objectInHand.Settings.weightType==ObjectSettings.ObjectWeight.heavy)
                    {
                        return;
                    }
                    //if object in hand is a tool
                    if (objectInHand.Settings.IsTool() && !concernedObject.Settings.IsTool())
                    {
                        concernedObject.SetHighlightColor(renderer.HighlightToolsColor);
                        concernedObject.Highlight(grabbedObject);
                        ToolSettings toolInHandSettings = (ToolSettings)objectInHand.Settings;
                        concernedObject.SetUIActionIcon(toolInHandSettings.ActionIcon);
                    }
                    //if object in hand is not a tool and raycasted object is a stationnary tool
                    else if(concernedObject.Settings.IsTool() && concernedObject.Settings.IsStationnaryTool())
                    {
                        if (concernedObject.Settings.NeedsToBePlugged()&& !concernedObject.GetComponent<ObjectState>().Plugged)
                        {
                            return;
                        }
                        ToolSettings stationnaryToolsSettings = (ToolSettings)concernedObject.Settings;
                        objectInHand.SetHighlightColor(renderer.HighlightToolsColor);
                        objectInHand.Highlight(concernedObject.gameObject);
                        objectInHand.SetUIActionIcon(stationnaryToolsSettings.ActionIcon);
                    }
                }
            }
        }
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
