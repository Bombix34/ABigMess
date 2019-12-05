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

    // [Header("Action when player interact without obj in hand")]
    //  [SerializeField] UnityEvent onInteractWithoutTool;


    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Define a canvas for InteractObject: " + name);
        }
        outline.OutlineWidth = 0;
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
            InteractObject toolObj = objPlayer.GetComponent<InteractObject>();
            if (toolObj.Settings.IsTool())
            {
                ToolSettings tool = (ToolSettings)toolObj.Settings;
                tool.ApplyEvent(this);
            }
        }
    }

    public void Grab()
    {
        grabbed = true;
        ResetHighlight();
    }

    public void Dropdown()
    {
        grabbed = false;
    }

    #region HIGHLIGHT_SYSTEM

    public void Highlight(GameObject grabbedObject)
    {
        Highlight((ToolSettings)grabbedObject.GetComponent<InteractObject>().Settings);
    }

    public void Highlight(ToolSettings grabbedObject)
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
    private void UpdateOverlayText(ToolSettings grabbedObject)
    {
        bool containsObjConcerned = false;

        if (grabbedObject != null && grabbedObject.IsTool())
        {
            if (settings != null)
            {
                if (grabbedObject.interactionsList.Count > 0)
                {
                    for (int index = 0; index < grabbedObject.interactionsList.Count; index++)
                    {
                        if (grabbedObject.interactionsList[index].objectConcerned == settings.objectType)
                        {
                            containsObjConcerned = true;
                            if (grabbedObject.interactionsList[index].eventsToLaunch != null && grabbedObject.interactionsList[index].eventsToLaunch.Count > 0)
                            {
                                interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetText(grabbedObject.interactionsList[index].eventsToLaunch[0].name);
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

    #endregion
}
