using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractObject : MonoBehaviour
{

    Canvas canvas; // The canvas where I display my button overlay 
    [SerializeField]
    GameObject interactButtonOverlayPrefab;
    GameObject interactButtonOverlayInstance;

    static float HOLD_TIME = 0.125f;
    float holdMaterial = HOLD_TIME;

    bool interacted;
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

    void Start()
    {
        outline = gameObject.AddComponent<Outline>();
        outline.OutlineMode = Outline.Mode.OutlineVisible;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if(canvas == null)
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
        if(decreaseOutline)
        {
            outline.OutlineWidth = outlineAnimation.Evaluate(outlineTime) * outlineWidth;
            if (outlineTime > 0)
            {
                outlineTime -= Time.deltaTime * outlineSpeed;
            }
        } else
        {
            outline.OutlineWidth = outlineAnimation.Evaluate(outlineTime) * outlineWidth;
            if (outlineTime < 1)
            {
                outlineTime += Time.deltaTime * outlineSpeed;
            }
        }
        UpdateOverlayPosition();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Interact();
        }
    }

    public void Interact()
    {
        interacted = true;
        ResetHighlight();
    }

    #region HIGHLIGHT_SYSTEM

    public void Highlight()
    {
        holdMaterial = HOLD_TIME;
        if (interactButtonOverlayInstance == null)
        {
            interactButtonOverlayInstance = Instantiate(interactButtonOverlayPrefab, canvas.transform);
            interactButtonOverlayInstance.GetComponent<InteractButtonOverlay>().SetText("Interact");
        } else
        {
            interactButtonOverlayInstance.SetActive(true);
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
}
