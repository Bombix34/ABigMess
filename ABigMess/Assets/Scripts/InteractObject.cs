using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class InteractObject : MonoBehaviour
{

    private Canvas canvas; // The canvas where I display my button overlay 

    [SerializeField]
    private GameObject interactButtonOverlayPrefab;
    private GameObject interactButtonOverlayInstance;

    [SerializeField]
    private ObjectSettings settings;
    [SerializeField]
    private OutlineSettings outlineSettings;

    private Rigidbody body;

    private static float HOLD_TIME = 0.125f;
    private float holdMaterial = HOLD_TIME;

    private bool grabbed = false;
    private bool childrenHaveMaterials;

    private SimpleOutline outline;

    private float outlineTime;
    private bool decreaseOutline = true;

    private float initMass;

    private List<GameObject> attachedPlayers;

    private void Awake()
    {
        attachedPlayers = new List<GameObject>();
        body = GetComponent<Rigidbody>();
        initMass = body.mass;
        body.isKinematic = false;
        outline = gameObject.AddComponent<SimpleOutline>();
        GameObject canvasObj = UIManager.Instance.gameObject;
        if (canvasObj != null)
        {
            canvas = canvasObj.GetComponent<Canvas>();
        }
        if (canvas == null)
        {
            Debug.LogError("Define a canvas for InteractObject: " + name);
        }
        if(this.GetComponent<ObjectState>()==null)
        {
            this.gameObject.AddComponent<ObjectState>();
        }
    }
    private void Start()
    {
        InitHighlight();
        SetupWeight();
        SceneObjectDatas.Instance.AddObject(this);
    }

    private void Update()
    {
        UpdateHighlight();
        UpdateOverlayPosition();
        UpdateFixedJoint();
    }

    /// <summary>
    /// Interact if I have a tool in hand
    /// </summary>
    /// <param name="grabbedObject">Is the tool</param>
    public void Interact(GameObject grabbedObject)
    {
        InteractObject toolObj = grabbedObject.GetComponent<InteractObject>();

        if (toolObj.Settings.IsTool() && !toolObj.Settings.NeedsToBePlugged())
        {
            ToolSettings tool = (ToolSettings)toolObj.Settings;
            StartCoroutine(tool.ApplyEvent(this));
        }
        else if (toolObj.Settings.IsTool() && toolObj.Settings.NeedsToBePlugged())
        {
            if (toolObj.GetComponent<ObjectState>() != null)
            {
                if (toolObj.GetComponent<ObjectState>().Plugged)
                {
                    ToolSettings tool = (ToolSettings)toolObj.Settings;
                    StartCoroutine(tool.ApplyEvent(this));
                }
            }
        }
        else
        {
            // if I'm a tool
            if (Settings.IsTool() && !Settings.NeedsToBePlugged())
            {
                ToolSettings tool = (ToolSettings)Settings;
                StartCoroutine(tool.ApplyEvent(toolObj));
            }
            else if (Settings.IsTool() && Settings.NeedsToBePlugged())
            {
                if (toolObj.GetComponent<ObjectState>() != null)
                {
                    if (toolObj.GetComponent<ObjectState>().Plugged)
                    {
                        ToolSettings tool = (ToolSettings)Settings;
                        StartCoroutine(tool.ApplyEvent(toolObj));
                    }
                }
            }
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        // Only if the object is not holded it can collide and apply his states to surrounding objects
        if (attachedPlayers.Count == 0)
        {
            if(settings==null)
            {
                return;
            }
            GameObject enteredObject = collision.gameObject;
            if (Settings.objectType == ObjectSettings.ObjectType.sponge && !Settings.NeedsToBePlugged() && enteredObject.GetComponent<InteractObject>() != null)
            {
                ToolSettings tool = (ToolSettings)Settings;
                tool.ApplyEvent(enteredObject.GetComponent<InteractObject>());
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
            StartCoroutine(noObjEvents.ApplyEvent(this));
        }
        else
        {
            Interact(objPlayer);
        }
    }

    private void SetupWeight()
    {
        if (settings == null)
        {
            Debug.Log("Settings of the object " + this.gameObject.name + " missing");
            return;
        }
        this.transform.parent = null;
        body.mass = initMass;
        body.useGravity = true;
        body.isKinematic = false;
    }

    public void Grab(GameObject player)
    {
        grabbed = true;
        attachedPlayers.Add(player);
        if (settings != null)
        {
            if (IsHeavy)
            {
                if (attachedPlayers.Count > 1)
                {
                    UpdateHeavyObjectBringed();
                }
                else
                {
                    attachedPlayers[0].GetComponent<PlayerManager>().Movement.CanMove = false;
                }
            }
            else
            {
                if (!settings.isOneHandedCarrying)
                {
                    body.constraints = RigidbodyConstraints.FreezeAll;  
                    body.useGravity = false;
                }
                else
                {
                    body.mass = 0f;
                }
            }
        }
        ResetHighlight();
    }

    public void UpdateHeavyObjectBringed()
    {
        MultiplayerBring bringSystem;
        if (this.gameObject.GetComponent<MultiplayerBring>() == null)
        {
            bringSystem = this.gameObject.AddComponent<MultiplayerBring>();
        }
        else
        {
            bringSystem = this.GetComponent<MultiplayerBring>();
        }
        foreach (var player in attachedPlayers)
        {
            bringSystem.UpdatePlayers(player, true);
        }
        bringSystem.SetMovementSettings(attachedPlayers[0].GetComponent<PlayerManager>().Reglages);
    }

    public void Dropdown(GameObject player)
    {
        grabbed = false;
        body.constraints = RigidbodyConstraints.None;
        attachedPlayers.Remove(player);
        if (IsHeavy)
        {
            MultiplayerBring bringSystem = this.gameObject.GetComponent<MultiplayerBring>();
            if (bringSystem != null)
            {
                bringSystem.UpdatePlayers(player, false);
                if (attachedPlayers.Count < 2)
                {
                    bringSystem.EndMovement();
                    bringSystem.DetachPlayer(player);
                    attachedPlayers[0].GetComponent<PlayerManager>().Movement.CanMove = false;
                }
            }
        }
        SetupWeight();
    }

    public void DetachPlayer()
    {
        if (IsHeavy)
        {
            return;
        }
        attachedPlayers[0].GetComponent<PlayerManager>().DropBringObject(true);
    }

    private void UpdateFixedJoint()
    {
        if(settings.isOneHandedCarrying && attachedPlayers.Count>0)
        {
            if(GetComponent<FixedJoint>()!=null && GetComponent<FixedJoint>().connectedBody==null)
            {
                GetComponent<FixedJoint>().connectedBody = transform.parent.GetComponent<Rigidbody>();
            }
        }
    }

    #region HIGHLIGHT_SYSTEM

    private void InitHighlight()
    {
        outline.OutlineMode = SimpleOutline.Mode.OutlineVisible;
        outline.OutlineWidth = 0;
        if(settings==null)
        {
            return;
        }
        if (settings.IsTool())
        {
            outline.OutlineColor = Color.green;
        }
        else
        {
            outline.OutlineColor = Color.white;
        }
    }

    public void SetHighlightColor(Color newColor)
    {
        outline.OutlineColor = newColor;
    }

    public void Highlight(GameObject grabbedObject)
    {
        Highlight(grabbedObject.GetComponent<InteractObject>().Settings);
    }

    public void Highlight(ObjectSettings grabbedObject)
    {
        holdMaterial = HOLD_TIME;
        //interactButtonOverlayInstance
        decreaseOutline = false;
    }

    public void SetUIActionIcon(Sprite newIcon)
    {
        if (interactButtonOverlayInstance == null)
        {
            interactButtonOverlayInstance = Instantiate(interactButtonOverlayPrefab, canvas.transform);
            //UpdateOverlayText(grabbedObject);
        }
        else
        {
            interactButtonOverlayInstance.SetActive(true);
            //UpdateOverlayText(grabbedObject);
        }
        interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetActionIcon(newIcon);
    }


    public void UpdateOverlayPosition()
    {
        if (interactButtonOverlayInstance != null)
        {
            Vector3 overlayPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, transform.localScale.y, 0));

            interactButtonOverlayInstance.transform.position = overlayPos;
        }
    }

    private void UpdateHighlight()
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
            outline.OutlineWidth = outlineSettings.outlineAnimation.Evaluate(outlineTime) * outlineSettings.outlineWidth;
            if (outlineTime > 0)
            {
                outlineTime -= Time.deltaTime * outlineSettings.outlineSpeed;
            }
        }
        else
        {
            outline.OutlineWidth = outlineSettings.outlineAnimation.Evaluate(outlineTime) * outlineSettings.outlineWidth;
            if (outlineTime < 1)
            {
                outlineTime += Time.deltaTime * outlineSettings.outlineSpeed;
            }
        }
    }

    public void ResetHighlight()
    {
        if (interactButtonOverlayInstance != null)
        {
            interactButtonOverlayInstance.SetActive(false);
        }
        outline.OutlineWidth = 0;
        decreaseOutline = true;
    }

    #endregion

    #region INTERACT_BUTTON_OVERLAY_TEXT

    /*
    private void UpdateOverlayText(ObjectSettings grabbedObject)
    {
        bool containsObjConcerned = false;
        InteractButtonOverlay interactButtonOverlay = interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>();

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
                                interactButtonOverlay.SetText(toolSettings.interactionsList[index].eventsToLaunch[0].name);
                                interactButtonOverlay.SetImage(toolSettings.interactionsList[index].eventsToLaunch[0].icon);
                            }
                            else
                            {
                                interactButtonOverlay.SetErrorText("No events set");
                            }
                        }
                    }
                }
                else
                {
                    interactButtonOverlay.SetErrorText("No interact set");
                }

                if (!containsObjConcerned)
                {
                    interactButtonOverlay.SetErrorText("No objConcerned");
                }
            }
            else
            {
                interactButtonOverlay.SetErrorText("No settings set");
            }
        }
        else
        {
            interactButtonOverlay.SetErrorText("Not a tool");
        }
    }
    */

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

    public ObjectSettings.ObjectWeight GetObjectWeight
    {
        get => settings.weightType;
    }

    public bool IsHeavy
    {
        get => (settings.weightType == ObjectSettings.ObjectWeight.heavy);
    }

    public int AttachedPlayersCount
    {
        get => attachedPlayers.Count;
    }



    #endregion
}
