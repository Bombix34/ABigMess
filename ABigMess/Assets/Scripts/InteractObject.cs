using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InteractObject : MonoBehaviour
{

    Canvas canvas; // The canvas where I display my button overlay 

    [SerializeField]
    GameObject interactButtonOverlayPrefab;
    GameObject interactButtonOverlayInstance;

    [SerializeField]
    ObjectSettings settings;
    Rigidbody body;

    static float HOLD_TIME = 0.125f;
    float holdMaterial = HOLD_TIME;

    bool grabbed = false;
    bool childrenHaveMaterials;

    Outline outline;

    [SerializeField]
    AnimationCurve outlineAnimation;

    float outlineTime;
    bool decreaseOutline = true;

    [SerializeField]
    [Range(1, 15)]
    float outlineWidth = 8;

    [SerializeField]
    [Range(1, 10)]
    float outlineSpeed = 2;

    private List<GameObject> attachedPlayers;

    // [Header("Action when player interact without obj in hand")]
    //  [SerializeField] UnityEvent onInteractWithoutTool;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        outline = gameObject.AddComponent<Outline>();
        attachedPlayers = new List<GameObject>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Define a canvas for InteractObject: " + name);
        }
    }
    void Start()
    {
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
        body.isKinematic = false;
      //  SetupWeight();
    }

    void Update()
    {
        holdMaterial -= Time.deltaTime;
        if (holdMaterial <= 0)
        {
            holdMaterial = HOLD_TIME;
            ResetHighlight();
        }
        // We start the counter by setting it to time.deltaTIme wich is > 0
        if (decreaseOutline)
        {
            outline.OutlineWidth = outlineAnimation.Evaluate(outlineTime) * outlineWidth;
            if (outlineTime > 0)
            {
                outlineTime -= Time.deltaTime * outlineSpeed;
            }
        }
        else
        {
            outline.OutlineWidth = outlineAnimation.Evaluate(outlineTime) * outlineWidth;
            if (outlineTime < 1)
            {
                outlineTime += Time.deltaTime * outlineSpeed;
            }
        }
        UpdateOverlayPosition();
    }

    public void Interact(GameObject grabbedObject)
    {
        InteractObject toolObj = grabbedObject.GetComponent<InteractObject>();
        if (toolObj.Settings.IsTool())
        {
            ToolSettings tool = (ToolSettings)toolObj.Settings;
            tool.ApplyEvent(this);
        }
        else
        {
            // if I'm a tool
            if (Settings.IsTool())
            {
                ToolSettings tool = (ToolSettings)Settings;
                tool.ApplyEvent(toolObj);
            }
        }
    }


    public void Interact(PlayerManager player)
    {
        ResetHighlight();
        GameObject objPlayer = player.GrabbedObject;
        if (objPlayer == null)
        {
            //no tools in hand
            ToolSettings noObjEvents = player.reglages.noObjectInHandEventsList;
            noObjEvents.ApplyEvent(this);
        }
        else
        {
            Interact(objPlayer);
        }
    }

    private void SetupWeight()
    {
        if(settings==null)
        {
            Debug.Log("Settings of the object "+ this.gameObject.name  +" missing");
            return;
        }
        body.useGravity = true;
        body.isKinematic = false;
    }
    
    public void Grab(GameObject player)
    {
        grabbed = true;
        attachedPlayers.Add(player);
        if(settings!=null)
        {
            if (IsHeavy()) 
            {
                if(attachedPlayers.Count > 1)
                {
                    //body.isKinematic = false;
                    if(attachedPlayers.Count==2)
                    {
                        SetupHeavyObjectBringed();
                    }
                    else
                    {
                        this.gameObject.GetComponent<MultiplayerBring>().UpdatePlayers(attachedPlayers);
                    }
                }
                else
                {
                    attachedPlayers[0].GetComponent<PlayerManager>().Movement.CanMove = false;
                }
            }
            else
            {
                //body.isKinematic = false;
                body.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                body.useGravity = false;
            }
        }
        else
        {
            body.isKinematic = false;
        }
        ResetHighlight();
    }

    public void SetupHeavyObjectBringed()
    {
        MultiplayerBring bringSystem = this.gameObject.AddComponent<MultiplayerBring>();
        bringSystem.UpdatePlayers(attachedPlayers);
        bringSystem.SetMovementSettings(attachedPlayers[0].GetComponent<PlayerManager>().Reglages);
    }

    public void Dropdown(GameObject player)
    {
        grabbed = false;
        body.constraints = RigidbodyConstraints.None;
        attachedPlayers.Remove(player);
        if(IsHeavy())
        {
            MultiplayerBring bringSystem = this.gameObject.GetComponent<MultiplayerBring>();
            if (bringSystem!=null)
            {
                bringSystem.UpdatePlayers(attachedPlayers);
                if (attachedPlayers.Count < 2 )
                {
                    this.GetComponent<MultiplayerBring>().EndMovement();
                    attachedPlayers[0].GetComponent<PlayerManager>().Movement.CanMove = false;
                }
            }
        }
        SetupWeight();
    }

    #region HIGHLIGHT_SYSTEM

    public void Highlight(GameObject grabbedObject)
    {
        Highlight(grabbedObject.GetComponent<InteractObject>().Settings);
    }

    public void Highlight(ObjectSettings grabbedObject)
    {
        holdMaterial = HOLD_TIME;
        if (interactButtonOverlayInstance == null)
        {
            interactButtonOverlayInstance = Instantiate(interactButtonOverlayPrefab, canvas.transform);
            UpdateOverlayText(grabbedObject);
        }
        else
        {
            interactButtonOverlayInstance.SetActive(true);
            UpdateOverlayText(grabbedObject);
        }
        decreaseOutline = false;
    }


    public void UpdateOverlayPosition()
    {
        if (interactButtonOverlayInstance != null)
        {
            Vector3 overlayPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.localScale.y, 0));

            interactButtonOverlayInstance.transform.position = overlayPos;
        }
    }

    public void ResetHighlight()
    {
        if (interactButtonOverlayInstance != null)
        {
            interactButtonOverlayInstance.SetActive(false);
            outline.OutlineWidth = 0;
            decreaseOutline = true;
        }

    }

    #endregion

    #region INTERACT_BUTTON_OVERLAY_TEXT
    private void UpdateOverlayText(ObjectSettings grabbedObject)
    {
        bool containsObjConcerned = false;

        if (grabbedObject != null && grabbedObject.IsTool())
        {
            ToolSettings toolSettings = (ToolSettings)grabbedObject;
            if (settings != null)
            {
                if (toolSettings.interactionsList.Count > 0)
                {
                    for (int index = 0; index < toolSettings.interactionsList.Count; index++)
                    {
                        if (toolSettings.interactionsList[index].objectConcerned == settings.objectType)
                        {
                            containsObjConcerned = true;
                            if (toolSettings.interactionsList[index].eventsToLaunch != null && toolSettings.interactionsList[index].eventsToLaunch.Count > 0)
                            {
                                interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetText(toolSettings.interactionsList[index].eventsToLaunch[0].name);
                            }
                            else
                            {
                                interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetErrorText("No events set");
                            }
                        }
                    }
                }
                else
                {
                    interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetErrorText("No interact set");
                }

                if (!containsObjConcerned)
                {
                    interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetErrorText("No objConcerned");
                }
            }
            else
            {
                interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetErrorText("No settings set");
            }
        }
        else
        {
            interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetErrorText("Not a tool");
        }
    }
    #endregion

    #region GET/SET

    public Vector3 Rotation
    {
        get
        {
            if (settings != null)
            {
                return settings.rotation;
            }
            else
            {
                Debug.LogError("No rotation defined for the object: " + name);
                return Vector3.zero;
            }
        }
    }

    public ObjectSettings Settings
    {
        get => settings;
    }

    public bool IsHeavy()
    {
        return (settings.weightType == ObjectSettings.ObjectWeight.heavy);
    }

    #endregion
}
